using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {   
        public string fen { get; private set; }
        Figure[,] figures;
        public Color moveColor { get; private set; }
        public int moveNumber { get; private set; }

        public string roque { get; private set; }
        public Board (string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        void Init()
        {
            //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            string[] parts = fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            moveColor = (parts[1] == "b") ? Color.black : Color.white;
            moveNumber = int.Parse(parts[5]);
            roque = parts[2];
        }

        void InitFigures (string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7-y][x] == '.' ? Figure.none : (Figure)lines[7 - y][x];
                
        }

        void GenerateFEN()
        {
            this.fen= FenFigures() + " " +
                   (moveColor == Color.black ? "w" : "b")
                   +" - - 0 " + moveNumber.ToString();
        }

        public IEnumerable<FigureOnCord> YieldFigures()
        {
            foreach (Cord cord in Cord.YieldCords())
                if (GetFigureAt(cord).GetColor() == moveColor)
                    yield return new FigureOnCord(GetFigureAt(cord), cord);
        }

        string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(figures[x, y] == Figure.none ? '1' : (char)figures[x, y]);
                if (y > 0)
                    sb.Append('/');
            }

            string eight = "11111111";
            for (int j=8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }
        public Figure GetFigureAt (Cord cord)
        {
            if (cord.OnBoard())
                return figures[cord.x, cord.y];
            return Figure.none;
        }

        void SetFigureAt(Cord cord, Figure figure)
        {
            if (cord.OnBoard())
                figures[cord.x, cord.y] = figure;
        }

        public Board Move (FigureMove fm)
        {
            Board next = new Board(fen);
            bool promflag = false;
            if (fm.promotion != Figure.none && ((fm.from.y == 6 && fm.to.y == 7)
                                            || (fm.from.y == 1 && fm.to.y == 0) 
                                            && (fm.figure == Figure.blackPawn || fm.figure == Figure.whitePawn)))
                promflag = true;
            next.SetFigureAt(fm.from, Figure.none);
            next.SetFigureAt(fm.to, promflag ? fm.promotion : fm.figure );
            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFEN();
            return next;
        }

        public bool IsCheck()
        {
            Board after = new Board(fen);
            after.moveColor = moveColor.FlipColor();
            return after.CheckToKing();
        }

        public bool CheckToKing()
        {
            Moves moves = new Moves(this);
            Cord enemyKing = FindEnemyKing();
            foreach (FigureOnCord fc in YieldFigures())
            {
                FigureMove fm = new FigureMove(fc, enemyKing);
                if (moves.CanMove(fm))
                    return true;
            }
            return false;
        }

        private Cord FindEnemyKing()
        {
            Figure enemyKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
            foreach (Cord cord in Cord.YieldCords())
                if (GetFigureAt(cord) == enemyKing)
                    return cord;
            return Cord.none;
        }
        public bool IsCheckAfterMove(FigureMove fm)
        {
            Board after = Move(fm);
            return after.CheckToKing();
        }
    }
}
