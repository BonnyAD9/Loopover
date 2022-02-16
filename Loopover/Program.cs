using Loopover.Holders;
using Loopover.Templates;
using Loopover.UIs;
using System;

namespace Loopover
{
    class Program
    {
        public static Random random = new();
        private static readonly Stats stats = new();
        private static readonly Blocks blocks = new(3, 3);

        static void Main(string[] args)
        {
            if ((Console.WindowHeight < 21) || (Console.WindowWidth < 80))
            {
                Console.WriteLine("Too small console window! Window must be at least 80 characters wide and 21 characters high");
                return;
            }
            Console.TreatControlCAsInput = true;
            stats.LoadFromJson();
            ResultMessage msg = ResultMessage.Next;
            Game3 g3 = new(stats, blocks);
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
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true;
            stats.SaveToJson();
        }
    }
}
