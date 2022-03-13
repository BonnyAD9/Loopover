using System;

namespace Loopover.Usefuls;

static class CWriter
{
    public static void Colored(ConsoleColor color, object text) => Colored(color, text.ToString());
    public static void Colored(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    public static void Colored(ConsoleColor color, object text, int x, int y) => Colored(color, text.ToString(), x, y);
    public static void Colored(ConsoleColor color, string text, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Colored(color, text);
    }

    public static void ColoredOff(ConsoleColor color, object text, int x, int y) => ColoredOff(color, text.ToString(), x, y);
    public static void ColoredOff(ConsoleColor color, string text, int x, int y)
    {
        Console.CursorLeft += x;
        Console.CursorTop += y;
        Colored(color, text);
    }

    public static void ResetColored(ConsoleColor color, object text) => ResetColored(color, text.ToString());
    public static void ResetColored(ConsoleColor color, string text)
    {
        Console.ResetColor();
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    public static void ResetColored(ConsoleColor color, object text, int x, int y) => ResetColored(color, text.ToString(), x, y);
    public static void ResetColored(ConsoleColor color, string text, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        ResetColored(color, text);
    }

    public static void ResetColoredOff(ConsoleColor color, object text, int x, int y) => ResetColoredOff(color, text.ToString(), x, y);
    public static void ResetColoredOff(ConsoleColor color, string text, int x, int y)
    {
        Console.CursorLeft += x;
        Console.CursorTop += y;
        ResetColored(color, text);
    }
}
