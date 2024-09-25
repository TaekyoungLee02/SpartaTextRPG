using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.GameManager
{
    internal class Point
    {
        public const int X = 36;
        public const int Y = 11;

        public int _x;
        public int _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void Draw(string draw)
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(draw);
        }
    }
}
