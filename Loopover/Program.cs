using Bny.Console;
using Loopover.Holders;
using Loopover.Templates;
using Loopover.UIs;
using System;

namespace Loopover;

class Program
{
    public static Random random = new();
    private static readonly Stats stats = new();
    private static Blocks blocks;
    private static (int x, int y) curPos;

    static void Main(string[] args)
    {
        int width = 3;
        int height = 3;
        bool pe = false;
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "-pe" or "--power-efficient":
                    pe = true;
                    break;
                default:
                {
                    var strs = args[0].Split('x');
                    if (strs.Length != 2 || !ushort.TryParse(strs[0], out ushort x) || !ushort.TryParse(strs[1], out ushort y))
                    {
                        Term.FormLine(Term.brightRed, "Invalid argument", Term.reset);
                        return;
                    }
                }
                    break;
            }
        }

        blocks = new(width, height, width * height > 9);
        curPos = Loopover.Usefuls.Convert.GetPosition();
        Console.Write(Term.altBufferOn);
        if ((Console.WindowHeight < 21) || (Console.WindowWidth < 80))
        {
            Console.WriteLine("Too small console window! Window must be at least 80 characters wide and 21 characters high");
            return;
        }
        Console.TreatControlCAsInput = true;
        stats.LoadFromJson();
        ResultMessage msg = ResultMessage.Next;
        Status.GetSize();
        Game3 g3 = new(stats, blocks);
        g3.powerEfficient = pe;
        Statistics statistics = new(stats);
        Func<ResultMessage>[] inLoop = new Func<ResultMessage>[] { g3.Play, statistics.Play };
        int pos = 0;
        while (msg != ResultMessage.Exit)
        {
            switch (msg = inLoop[pos]())
            {
                case ResultMessage.Next:
                    pos = (pos + 1) % inLoop.Length;
                    break;
                case ResultMessage.Exit:
                    Exit();
                    return;
                case ResultMessage.Error:
                    Exit();
                    Console.WriteLine("An error occured");
                    return;
                default:
                    goto case ResultMessage.Error;
            }
        }
    }

    static void Exit()
    {
        Term.Form(Term.softReset, Term.altBufferOff, Term.move, curPos.x, curPos.y);
        stats.SaveToJson();
    }
}
