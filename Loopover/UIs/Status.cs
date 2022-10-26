using Bny.Console;
using Loopover.Usefuls;
using System;

namespace Loopover.UIs;

static class Status
{
    public static (int x, int y) Size { get; private set; }
    public static void Write(object o) => Write(o.ToString());
    public static void Write(string s)
    {
        (int x, int y) = Size;
        Term.Form(Term.move, x - 40, y - 1, Term.brightBlack, s.PadLeft(38)[..38]);
    }

    public static void WriteHelp(string s)
    {
        (int x, int y) = Size;
        int length = x - 42;
        Term.Form(Term.move, 2, y - 1, Term.brightBlack, s.PadRight(length)[..length]);
    }

    public static (int x, int y) GetSize() => Size = Loopover.Usefuls.Convert.GetWindowSize();
}
