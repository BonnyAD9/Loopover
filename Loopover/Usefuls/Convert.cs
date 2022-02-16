using Loopover.Holders;
using System;

namespace Loopover.Usefuls
{
    static class Convert
    {
        public static string ToString(Direction direction, bool rotate) => (direction, rotate) switch
        {
            (Direction.Left, true) => "L ",
            (Direction.Right, true) => "R ",
            (Direction.Up, true) => "U ",
            (Direction.Down, true) => "D ",
            (Direction.Left, false) => "l ",
            (Direction.Right, false) => "r ",
            (Direction.Up, false) => "u ",
            (Direction.Down, false) => "d ",
            _ => "? "
        };

        public static string[] SplitAfter(string s, int num)
        {
            string[] ret = new string[(int)Math.Ceiling(s.Length / (double)num)];
            int i;
            for (i = 0; i < (ret.Length - 1); i++)
            {
                ret[i] = s[(i * num)..((i + 1) * num)];
            }
            ret[i] = s[(i * num)..];
            return ret;
        }

        public static Direction Reverse(Direction direction) => direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => direction
        };
    }
}
