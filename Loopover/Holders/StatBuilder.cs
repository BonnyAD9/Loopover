using System;
using System.Text;
using Convert = Loopover.Usefuls.Convert;

namespace Loopover.Holders;

class StatBuilder
{
    public int StartX { get; set; }
    public int StartY { get; set; }

    private readonly StringBuilder moves = new();
    private int[] scramble;

    public TimeSpan Time { get; set; }

    public StatBuilder() { }

    public int NumMoves { get; private set; } = 0;

    public StatBuilder(int startX, int startY, int[] scramble) => Reset(startX, startY, scramble);

    public void Reset(int startX, int startY, int[] scramble)
    {
        StartX = startX;
        StartY = startY;
        Reset(scramble);
    }

    public void Reset(int[] scramble)
    {
        this.scramble = scramble;
        NumMoves = 0;
        moves.Clear();
    }

    public void SetStart(int x, int y)
    {
        StartX = x;
        StartY = y;
    }

    public void Move(Direction direction, bool rotate)
    {
        if (rotate)
            NumMoves++;
        moves.Append(Convert.ToString(direction, rotate));
    }

    public Stat ToStat(TimeSpan time) => new(StartX, StartY, moves.ToString(), scramble, time, NumMoves);
}
