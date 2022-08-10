using Bny.Console;
using Loopover.Usefuls;
using System;

namespace Loopover.UIs;

static class Status
{
    public static void Write(object o) => Write(o.ToString());
    public static void Write(string s)
    {
        (int x, int y) = Term.GetWindowSize();
        Term.Form(Term.move, x - 40, y - 1, Term.brightBlack, s.PadLeft(38)[..38]);
    }

    public static void WriteHelp(string s)
    {
        (int x, int y) = Term.GetWindowSize();
        int length = x - 42;
        Term.Form(Term.move, 2, y - 1, Term.brightBlack, s.PadRight(length)[..length]);
    }
}
