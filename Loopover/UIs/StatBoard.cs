using Bny.Console;
using Loopover.Holders;
using Loopover.Usefuls;
using System;

namespace Loopover.UIs;

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

    public void UpdateTime() => Term.Form(Term.move, DrawPosition, Blocks.OffsetY, Term.brightYellow, $"{Stats.Time.TotalSeconds:00.000}   ");

    public void UpdateMoves() => Term.Form(Term.move, DrawPosition + 1, Blocks.OffsetY + 1, Term.white, $"{Stats.Moves} moves   ");

    public void Update()
    {
        UpdateTime();
        UpdateMoves();
        UpdateHistory();
    }

    public void UpdateHistory()
    {
        var sb = Term.PrepareSB(
            Term.reset, Term.move, DrawPosition, Blocks.OffsetY + 2,
            Term.brightBlack, Stats.Count,
            Term.brightGreen, $" {Stats.Best.time.TotalSeconds:00.000}",
            Term.green, $" {Stats.Best.numMoves}   "
        );

        for (int i = Stats.Count - 1; (i >= 0) && ((Stats.Count - (i - 2)) < Blocks.CharHeight); i--)
        {
            sb.Append(Term.Prepare(
                Term.column, DrawPosition, Term.down1,
                Term.brightWhite, $"{Stats[i].time.TotalSeconds:00.000} ",
                Term.brightBlack, $"{Stats[i].numMoves}   "));
        }

        Console.Write(sb.ToString());
    }

}
