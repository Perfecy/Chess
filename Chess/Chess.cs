using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Chess
    {
        public string fen { get; private set; }
        Board board;
        Moves moves;
        List<FigureMove> allMoves;
        public int mate;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
            
        }

        Chess(Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }

        public Chess Move(string move)//Pe2e4  Pe7e8Q
        {
            FigureMove fm = new FigureMove(move);
            if (!moves.CanMove(fm))
                     return this;
            if (board.IsCheckAfterMove(fm))
                return this;
            Board nextBoard = board.Move(fm);

            Chess nextChess = new Chess(nextBoard);

            //nextChess.fen = board.fen
            return nextChess;
        }

        public char GetFigureAt (int x, int y)
        {
            Cord cord = new Cord(x, y);
            Figure f = board.GetFigureAt(cord);
            return f == Figure.none ? '.' : (char)f;
        }

        public void FindAllMoves()
        {
            allMoves = new List<FigureMove>();
            foreach (FigureOnCord fc in board.YieldFigures())
                foreach (Cord to in Cord.YieldCords())
                {
                    FigureMove fm = new FigureMove(fc, to);
                    if (moves.CanMove(fm))
                        if(!board.IsCheckAfterMove(fm))
                            allMoves.Add(fm);
                }
           
        }
        
        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMove fm in allMoves)
                list.Add(fm.ToString());
            mate = allMoves.Count;
            return list;
        }

        public bool IsCheck()
        {
            return board.IsCheck();
        }

    }
}
