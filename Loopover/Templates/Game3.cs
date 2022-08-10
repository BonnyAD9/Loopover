using Bny.Console;
using Loopover.Holders;
using Loopover.UIs;
using System;

namespace Loopover.Templates;

class Game3
{
    private readonly Blocks blocks;
    private readonly Stats stats = new();
    private readonly StatBoard statb;
    private ConsoleKeyInfo cki = new('a', ConsoleKey.A, false, false, false);
    private bool ready = false;
    public delegate void Function();
    private readonly Function Reset;
    private readonly Function Scramble;
    private bool AllowSave { get; set; } = true;
    private readonly string help = "[Enter]Scramble [Backspace]Pause/Resume [R]Reload [S]Save [Esc]Exit [Tab]Stats [Arrows]Move [Ctrl]Rotate";
    private (int x, int y) lastSize;

    public Game3(Stats stats, Blocks blocks)
    {
        this.stats = stats;
        this.blocks = blocks;
        statb = new(blocks, 2, stats);
        Reset = () =>
        {
            blocks.SetupCenter();
            Status.WriteHelp(help);
        };
        Scramble = blocks.ScrambleDraw;
        lastSize = Term.GetWindowSize();
    }

    public Game3(Stats stats, Blocks blocks, Function reset, Function scramble, bool timeStart, bool allowSave, string help)
    {
        this.stats = stats;
        this.blocks = blocks;
        statb = new(blocks, 2, stats);
        Reset = () =>
        {
            reset();
            Status.WriteHelp(help);
        };
        Scramble = scramble;
        ready = timeStart;
        AllowSave = allowSave;
    }

    public ResultMessage Play()
    {
        Reset();
        statb.Update();
        Status.Write("Game loaded");
        ResultMessage rm = ResultMessage.None;
        while (rm == ResultMessage.None)
        {
            if (Console.KeyAvailable)
                rm = NewPress();
            /*var newSize = Term.GetWindowSize();
            if (lastSize != newSize)
            {
                lastSize = newSize;
                Reset();
            }*/
            statb.UpdateTime();
        }
        return rm;
    }

    private ResultMessage NewPress()
    {
        cki = Console.ReadKey(true);
        (int x, int y) = (blocks.SelX, blocks.SelY);
        (Direction direction, bool rotate) = GetDirection();
        if (direction == Direction.None)
        {
            switch (cki.Key)
            {
                case ConsoleKey.Enter:
                    stats.Ingame = false;
                    Scramble();
                    ready = true;
                    stats.NewStat(blocks);
                    Status.Write("Reseted board");
                    break;
                case ConsoleKey.Backspace:
                    Status.Write("Paused");
                    while (Console.ReadKey(true).Key != ConsoleKey.Backspace) ;
                    Reset();
                    statb.Update();
                    Status.Write("Resumed");
                    break;
                case ConsoleKey.R:
                    Reset();
                    statb.Update();
                    Status.Write("Refreshed view");
                    break;
                case ConsoleKey.S:
                    if (!AllowSave)
                        break;
                    if (stats.SaveToJson())
                        Status.Write("Saved to file");
                    else
                        Status.Write("Save failed");
                    break;
                case ConsoleKey.Escape:
                    return ResultMessage.Exit;
                case ConsoleKey.Tab:
                    return ResultMessage.Next;
            }
            return ResultMessage.None;
        }
        blocks.Play(rotate, direction);
        if (rotate)
        {
            if (ready)
            {
                stats.Ingame = true;
                stats.SetStart(x, y);
                ready = false;
                Status.Write("Game started");
            }
            if (stats.Ingame && blocks.CheckWin())
            {
                stats.Ingame = false;
                stats.Move(direction, rotate);
                if (blocks.Width == 3 && blocks.Height == 3)
                    stats.SaveTime();
                else
                {

                }
                statb.UpdateMoves();
                statb.UpdateHistory();
                Status.Write("Game solved");
            }
        }
        if (stats.Ingame)
        {
            stats.Move(direction, rotate);
            if (rotate)
                statb.UpdateMoves();
        }
        return ResultMessage.None;
    }

    private (Direction, bool) GetDirection()
    {
        bool rotate = (cki.Modifiers & ConsoleModifiers.Control) != 0;
        return cki.Key switch
        {
            ConsoleKey.LeftArrow => (Direction.Left, rotate),
            ConsoleKey.RightArrow => (Direction.Right, rotate),
            ConsoleKey.UpArrow => (Direction.Up, rotate),
            ConsoleKey.DownArrow => (Direction.Down, rotate),
            _ => (Direction.None, false)
        };
    }

}
