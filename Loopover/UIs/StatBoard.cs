using Loopover.Holders;
using Loopover.Usefuls;
using System;

namespace Loopover.UIs
{
    class StatBoard
    {
        public Blocks Blocks { get; init; }
        public Stats Stats { get; init; }

        public int Offset { get; private set; }
        private int DrawPosition => Offset + Blocks.OffsetX + Blocks.CharWidth;

        public StatBoard(Blocks blocks, int offset, Stats stats)
        {
            Blocks = blocks;
            Offset = offset;
            Stats = stats;
        }

        public void UpdateTime() => CWriter.ResetColored(ConsoleColor.Yellow, $"{Stats.Time.TotalSeconds:00.000}   ", DrawPosition, Blocks.OffsetY);

        public void UpdateMoves() => CWriter.ResetColored(ConsoleColor.Gray, $"{Stats.Moves} moves   ", DrawPosition + 1, Blocks.OffsetY + 1);

        public void Update()
        {
            UpdateTime();
            UpdateMoves();
            UpdateHistory();
        }

        public void UpdateHistory()
        {
            CWriter.ResetColored(ConsoleColor.DarkGray, Stats.Count, DrawPosition, Blocks.OffsetY + 2);
            CWriter.Colored(ConsoleColor.Green, $" {Stats.Best.time.TotalSeconds:00.000}");
            CWriter.Colored(ConsoleColor.DarkGreen, $" {Stats.Best.numMoves}   ");
            Console.SetCursorPosition(DrawPosition, Blocks.OffsetY + 2);
            for (int i = Stats.Count - 1; (i >= 0) && ((Stats.Count - (i - 2)) < Blocks.CharHeight); i--)
            {
                CWriter.Colored(ConsoleColor.White, $"{Stats[i].time.TotalSeconds:00.000} ", DrawPosition, Console.CursorTop + 1);
                CWriter.Colored(ConsoleColor.DarkGray, $"{Stats[i].numMoves}   ");
            }
        }

    }
}
