using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Moves
    {
        FigureMove fm;
        Board board;

        public Moves (Board board)
        {
            this.board = board;
        }

        public bool CanMove (FigureMove fm)
        {
            this.fm = fm;
            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }

        bool CanMoveFrom()
        {
            return fm.from.OnBoard() && fm.figure.GetColor() == board.moveColor;
        }

        bool CanMoveTo()
        {
            return fm.to.OnBoard() && board.GetFigureAt(fm.to).GetColor() != board.moveColor && fm.from!= fm.to;
        }

        bool CanFigureMove()
        {
            switch (fm.figure)
            {
                case Figure.blackKing:
                case Figure.whiteKing:
                    return CanKingMove();

                case Figure.blackQueen:
                case Figure.whiteQueen:
                    return CanStraightMove();

                case Figure.blackRook:
                case Figure.whiteRook:
                    return (fm.SignX == 0 || fm.SignY == 0) && CanStraightMove();

                case Figure.blackBishop:
                case Figure.whiteBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) && CanStraightMove();

                case Figure.blackKnight:
                case Figure.whiteKnight:
                    return CanKnightMove();
                   
                case Figure.blackPawn:
                case Figure.whitePawn:
                    return CanPawnMove();

                default: return false;
            }
        }
        
        private bool CanPawnMove()
        {
            if (fm.from.y == 0 || fm.from.y == 7)
                return false;
            int turn = fm.figure.GetColor() == Color.white ? 1 : -1;
            return CanPawnStep(turn) || CanPawnStepStep(turn) || CanPawnEat(turn);
        }

        private bool CanPawnEat(int turn)
        {
            if (board.GetFigureAt(fm.to) != Figure.none)
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == turn)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;

        }

        private bool CanPawnStepStep(int turn)
        {
            if (board.GetFigureAt(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == 2 * turn)
                        if (fm.from.y == 1 || fm.from.y == 6)
                            if (board.GetFigureAt(new Cord(fm.from.x, fm.from.y + turn)) == Figure.none)
                                return true;
                            else
                                return false;
                        else
                            return false;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        private bool CanPawnStep(int turn)
        {
            if (board.GetFigureAt(fm.to) == Figure.none)//maybe here 2.5
                if (fm.DeltaX == 0 && fm.DeltaY == turn)
                    return true; //if (fm.promotion != Figure.none && ((fm.from.y == 6 && fm.to.y == 7)
                                           // || (fm.from.y == 1 && fm.to.y == 0)
                                           // && (fm.figure == Figure.blackPawn || fm.figure == Figure.whitePawn)))
                else
                    return false;
            else
                return false;
                    
        }

        private bool CanStraightMove()
        {
            Cord at = fm.from;
            do
            {
                at = new Cord(at.x + fm.SignX, at.y + fm.SignY);
                if (at == fm.to)
                    return true;
            } while (at.OnBoard() && board.GetFigureAt(at) == Figure.none);
            return false;
        }

        private bool CanKingMove()
        {
            if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1)
                return true;
            else
                 return false;
        }

        private bool CanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2)
                return true;
            else if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1)
                return true;
            else
                return false;
        }

        private bool CanKingRoque()
        {
            return true;
        }
    }
}
