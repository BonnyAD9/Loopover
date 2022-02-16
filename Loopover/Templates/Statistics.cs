using Loopover.Holders;
using Loopover.UIs;
using System;

namespace Loopover.Templates
{
    class Statistics
    {
        private Stats Stats { get; set; }
        private StatViewer StatViewer { get; set; }
        private string help = "[Enter]TryScramble [+]NextMove [-]MoveBack [Space]ResetMoves [R]Reload [S]Save [Tab]Game [Esc]Exit [Down] [Up] [PgDn] [PgUp] [Home] [End]";

        public Statistics(Stats stats)
        {
            Stats = stats;
            StatViewer = new(Stats);
        }

        public ResultMessage Play()
        {
            StatViewer.CenterSetup();
            Status.WriteHelp(help);
            Status.Write("Stats loaded");
            ConsoleKeyInfo cki;
            while ((cki = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (cki.Key)
                {
                    case ConsoleKey.Enter:
                        StatViewer.ResetMoves();
                        StatViewer.PlayScramble();
                        Status.WriteHelp(help);
                        break;
                    case ConsoleKey.DownArrow:
                        StatViewer.Next(1);
                        Status.Write("Moved down");
                        break;
                    case ConsoleKey.UpArrow:
                        StatViewer.Next(-1);
                        Status.Write("Moved moved up");
                        break;
                    case ConsoleKey.PageDown:
                        StatViewer.Next(StatViewer.TextHeight);
                        Status.Write("Moved one page down");
                        break;
                    case ConsoleKey.PageUp:
                        StatViewer.Next(-StatViewer.TextHeight);
                        Status.Write("Moved one page up");
                        break;
                    case ConsoleKey.Home:
                        StatViewer.Top();
                        Status.Write("Moved to top");
                        break;
                    case ConsoleKey.End:
                        StatViewer.Bottom();
                        Status.Write("Moved to end");
                        break;
                    case ConsoleKey.Add or ConsoleKey.OemPlus:
                        StatViewer.Move(true);
                        Status.Write("Played next move");
                        break;
                    case ConsoleKey.D1: // I don't know why but this is what my Numpad Plus key returns
                        if (cki.KeyChar == '+')
                        {
                            StatViewer.Move(true);
                            Status.Write("Played next move");
                        }
                        break;
                    case ConsoleKey.Subtract or ConsoleKey.OemMinus:
                        StatViewer.Move(false);
                        Status.Write("Played previous move");
                        break;
                    case ConsoleKey.Spacebar:
                        StatViewer.ResetMoves();
                        Status.Write("Reseted moves");
                        break;
                    case ConsoleKey.S:
                        if (Stats.SaveToJson())
                            Status.Write("Saved");
                        else
                            Status.Write("Save failed");
                        break;
                    case ConsoleKey.R:
                        StatViewer.CenterSetup();
                        Status.WriteHelp(help);
                        Status.Write("Reloaded");
                        break;
                    case ConsoleKey.Tab:
                        return ResultMessage.Next;
                    /*default:
                        Status.Write(cki.Key);
                        break;*/
                }
            }
            return ResultMessage.Exit;
        }
    }
}
