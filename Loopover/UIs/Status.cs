using Loopover.Usefuls;
using System;

namespace Loopover.UIs;

static class Status
{
    public static int Position => Console.WindowHeight - 1;

    public static void Write(object o) => Write(o.ToString());
    public static void Write(string s) => CWriter.ResetColored(ConsoleColor.DarkGray, s.PadLeft(38)[..38], Console.WindowWidth - 40, Position);

    public static void WriteHelp(string s)
    {
        int length = Console.WindowWidth - 42;
        CWriter.ResetColored(ConsoleColor.DarkGray, s.PadRight(length)[..length], 2, Position);
    }
}
