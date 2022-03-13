using Loopover.Holders;
using Loopover.Usefuls;
using System;
using System.Linq;
using Convert = Loopover.Usefuls.Convert;

namespace Loopover.UIs;

class MoveReplay
{
    private StatViewer StatViewer { get; set; }
    private (Direction Direction, bool Rotate)[] Moves { get; set; }

    public int Position { get; private set; } = -1;

    public (Direction Direction, bool Rotate) this[int i]
    {
        get
        {
            if ((i < 0) || (i >= Moves.Length))
                return (Direction.None, false);
            return Moves[i];
        }
        private set => Moves[i] = value;
    }

    public MoveReplay(string moves, StatViewer statViewer)
    {
        StatViewer = statViewer;
        Moves = moves.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(MoveParse).ToArray();
        if (Moves.Length == 0)
            throw new ArgumentException("moves had 0 moves");
    }

    public bool Move(int num)
    {
        int val = num + Position;
        if (val < -1)
        {
            SetPos(-1);
            return false;
        }
        if (val >= Moves.Length)
        {
            SetPos(Moves.Length - 1);
            return false;
        }
        SetPos(val);
        return true;
    }

    public void SetPos(int pos)
    {
        Position = pos;
        DrawAll();
    }

    public void Reset() => SetPos(-1);

    public void DrawAll()
    {
        int count = StatViewer.RightTextLength / 2;
        int lcount = StatViewer.TextHeight - 14;
        int x = StatViewer.RightTextStart;
        int y = StatViewer.TopLengthStart + 4;
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int i = 0; i < lcount; i++)
        {
            Console.SetCursorPosition(x, y + i);
            for (int j = 0; j < count; j++)
            {
                int pos = (i * count) + j;
                if (pos >= Moves.Length)
                    Console.Write("  ");
                else if (pos == Position)
                {
                    CWriter.Colored(ConsoleColor.DarkCyan, Convert.ToString(Moves[pos].Direction, Moves[pos].Rotate));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else Console.Write(Convert.ToString(Moves[pos].Direction, Moves[pos].Rotate));
            }
        }
    }

    private (Direction, bool) MoveParse(string s) => s switch
    {
        "L" => (Direction.Left, true),
        "R" => (Direction.Right, true),
        "U" => (Direction.Up, true),
        "D" => (Direction.Down, true),
        "l" => (Direction.Left, false),
        "r" => (Direction.Right, false),
        "u" => (Direction.Up, false),
        "d" => (Direction.Down, false),
        _ => throw new FormatException("String had incorrect format")
    };
}
