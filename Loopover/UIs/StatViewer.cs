using Loopover.Holders;
using System;
using System.Text;
using Loopover.Usefuls;
using Loopover.Templates;

namespace Loopover.UIs
{
    class StatViewer
    {
        public const int width = 80;
        public const int height = 20;
        public const int leftLine = 49;

        public static int LeftTextLength => width - 4 - leftLine;
        public static int RightTextLength => leftLine - 4;
        public int LeftTextStart => OffsetX + 2;
        public int RightTextStart => OffsetX + LeftTextLength + 6;
        public int TopLengthStart => OffsetY + 2;
        public static int TextHeight => height - 3;

        public Stat Selection => Stats[^(Selected + 1)];

        public Blocks Blocks { get; private set; }

        public MoveReplay MoveReplay { get; private set; }

        private int selected;
        public int Selected
        {
            get => selected;
            private set
            {
                if (value < 0)
                    selected = 0;
                else if (value >= Stats.Count)
                    selected = Stats.Count - 1;
                else
                    selected = value;
            }
        }

        private int position;
        public int Position
        {
            get
            {
                if ((position + (TextHeight - 1)) < Selected)
                {
                    position = selected - (TextHeight - 1);
                    return position;
                }
                if (Selected < position)
                {
                    position = Selected;
                    return position;
                }
                return position;
            }
            private set
            {
                if ((value < 0) || (value >= Stats.Count))
                    return;
                position = value;
            }
        }

        public int OffsetX { get; private set; } = 0;
        public int OffsetY { get; private set; } = 0;

        public static int CenteredX => (Console.WindowWidth / 2) - (width / 2);
        public static int CenteredY => (Console.WindowHeight / 2) - (height / 2);

        private Stats Stats { get; set; }

        public StatViewer(Stats stats)
        {
            Stats = stats;
            Blocks = new(3, 3, true);
            SelectNew();
        }

        public void CenterSetup()
        {
            Console.CursorVisible = false;
            Center();
            Blocks.Reoffset(RightTextStart, TopLengthStart + 8);
            DrawAll();
            SelectNew();
        }

        public void DrawAll()
        {
            Console.ResetColor();
            Console.Clear();
            DrawFrame();
            DrawElements();
            DrawContent();
        }

        private void Center()
        {
            OffsetX = CenteredX;
            OffsetY = CenteredY;
        }

        public ResultMessage PlayScramble()
        {
            Game3 g3 = new(new(), Blocks, Redraw, () => Blocks.SetTemplateDraw(Selection.scramble, Selection.startX, Selection.startY), true, false,
                "[Enter]Scramble [Backspace]Pause/Resume [R]Reload [S]Save [Esc]Back [Arrows]Move [Ctrl]Rotate");
            var ret = g3.Play();
            Blocks.SetTemplateDraw(Selection.scramble, Selection.startX, Selection.startY);
            DrawAll();
            return ret;
        }

        public void Redraw()
        {
            Blocks.Reoffset(RightTextStart, TopLengthStart + 8);
            DrawAll();
        }

        public void DrawLine()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(RightTextStart - 2, TopLengthStart - 1);
            int bar = (TextHeight + 1) * Position / Stats.Count;
            for (int i = 0; i < (TextHeight + 1); i++)
            {if (i == bar)
                    Console.Write('█');
                else
                    Console.Write('│');
                Console.CursorLeft--;
                Console.CursorTop++;
            }
        }

        public void DrawContent()
        {
            CWriter.ResetColored(ConsoleColor.White, $"ID:    ", RightTextStart, TopLengthStart - 1);
            CWriter.Colored(ConsoleColor.DarkMagenta, $"{Stats.Count - Selected}       ");
            CWriter.Colored(ConsoleColor.White, $"Time:  ", RightTextStart, TopLengthStart);
            CWriter.Colored(ConsoleColor.DarkGreen, $"{Selection.time.TotalSeconds:00.000}   ");
            CWriter.Colored(ConsoleColor.White, $"Moves: ", RightTextStart, TopLengthStart + 1);
            CWriter.Colored(ConsoleColor.DarkRed, $"{Selection.numMoves}   ");
            CWriter.Colored(ConsoleColor.White, $"Date:  ", RightTextStart, TopLengthStart + 2);
            CWriter.Colored(ConsoleColor.DarkYellow, $"{Selection.date:g}     ");
            CWriter.Colored(ConsoleColor.White, $"Solve (start on {Selection.scramble[(Selection.startY * 3) + Selection.startX]}):", RightTextStart, TopLengthStart + 3);
            MoveReplay.DrawAll();
            CWriter.Colored(ConsoleColor.White, $"Scramble:", RightTextStart, TopLengthStart + 7);
            Blocks.DrawAll();
        }

        public void Move(bool forward)
        {
            (Direction Direction, bool Rotate) = MoveReplay[MoveReplay.Position];
            if (forward && MoveReplay.Move(1))
                Blocks.Play(MoveReplay[MoveReplay.Position].Rotate, MoveReplay[MoveReplay.Position].Direction);
            else if (!forward && MoveReplay.Move(-1) && (Direction != Direction.None))
                Blocks.Play(Rotate, Usefuls.Convert.Reverse(Direction));
        }

        public void ResetMoves()
        {
            MoveReplay.Reset();
            Blocks.SetTemplateDraw(Selection.scramble, Selection.startX, Selection.startY);
        }

        public void Next(int num)
        {
            Selected += num;
            DrawElements();
            SelectNew();
            DrawContent();
        }

        public void Top()
        {
            Selected = 0;
            DrawElements();
            SelectNew();
            DrawContent();
        }

        public void Bottom()
        {
            Selected = Stats.Count - 1;
            DrawElements();
            SelectNew();
            DrawContent();
        }

        public void SelectNew()
        {
            Blocks.SetTemplateDraw(Selection.scramble, Selection.startX, Selection.startY);
            Blocks.DrawAll();
            MoveReplay = new(Selection.moves, this);
        }

        public void DrawElements()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = Position; (i < (Position + TextHeight)) && (i < Stats.Count); i++)
            {
                if (i == Selected)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(LeftTextStart, TopLengthStart + (i - Position));
                Console.Write($"{Stats[^(i + 1)].date:d}".PadRight(12));
                Console.Write($"{Stats[^(i + 1)].time.TotalSeconds:00.000}".PadRight(11));
                Console.Write($"{Stats[^(i + 1)].numMoves}".PadRight(5));
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            DrawLine();
        }

        private void DrawFrame()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(OffsetX, OffsetY);
            for (int i = 0; i < height; i++)
            {
                Console.Write("█");
                Console.CursorLeft += width - 2;
                Console.Write("█");
                Console.SetCursorPosition(OffsetX, OffsetY + i + 1);
            }
            Console.SetCursorPosition(OffsetX + 1, OffsetY);
            Console.Write(new StringBuilder().Append('▀', width - 2));
            Console.SetCursorPosition(OffsetX + 1, OffsetY + height - 1);
            Console.Write(new StringBuilder().Append('▄', width - 2));
            Console.SetCursorPosition(LeftTextStart, TopLengthStart - 1);
            Console.Write("Date        Time       Moves");
            DrawLine();
        }

    }
}
