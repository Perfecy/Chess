using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class FigureMove
    {
        public Figure figure { get; private set; }
        public Cord from { get; private set; }
        public Cord to { get; private set; }
        public Figure promotion { get; private set; }

        public FigureMove(FigureOnCord fc, Cord to, Figure promotion = Figure.none)
        {
            this.figure = fc.figure;
            this.from = fc.cord;
            this.to = to;
            this.promotion = promotion;

        }

        public FigureMove (string move)//Pe2e4  //Pe7e8Q
        {
            this.figure = (Figure)move[0];
            this.from = new Cord(move.Substring(1, 2));
            this.to = new Cord(move.Substring(3, 2));
            this.promotion = (move.Length == 6) ? (Figure)move[5] : Figure.none;
            
        }

        public int DeltaX { get { return to.x - from.x; } }
        public int DeltaY { get { return to.y - from.y; } }

        public int AbsDeltaX { get { return Math.Abs(DeltaX); } }
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } }

        public int SignX { get { return Math.Sign(DeltaX); } }
        public int SignY { get { return Math.Sign(DeltaY); } }

        public override string ToString()
        {
            string text = (char)figure + from.Name + to.Name;
            if (promotion != Figure.none)
                text += (char)promotion;
            return text;
        }

    }
}
