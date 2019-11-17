using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Chess.Chess chess = new Chess.Chess();

            while(true)
            { //266%
                Console.WriteLine(chess.fen);
                Console.WriteLine(ChessToAscii(chess));
                Console.WriteLine(chess.IsCheck() ? "CHECK" : "-");
                foreach (string moves in chess.GetAllMoves())
                    Console.Write(moves + "\t");
                if (chess.mate == 0)
                {
                    Console.WriteLine("MATE");
                }
                Console.WriteLine();
                Console.Write("your turn>>> ");
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii(Chess.Chess chess)
        {
            string text = "  +-----------------+\n";
            for (int y = 7; y >= 0; y --)
            {
                text += y + 1;
                text += '|';
                for (int x = 0; x < 8; x++)
                {
                    text += chess.GetFigureAt(x, y) + " ";
                }
                text += "|\n";
                
            }
            text += "  +-----------------+\n";
            text += "  a b c d e f g h\n";
            return text;
        }
    }
}
