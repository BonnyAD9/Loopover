﻿using Loopover.Holders;
using System;
using System.Text;
using Loopover.Usefuls;
using Loopover.Templates;
using Bny.Console;

namespace Loopover.UIs;

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

    public static (int x, int y) Centered((int width, int height) size)
    {
        return ((size.width / 2) - (width / 2), (size.height / 2) - (height / 2));
    }

    private Stats Stats { get; set; }

    public StatViewer(Stats stats)
    {
        Stats = stats;
        Blocks = new(3, 3, true);
        SelectNew();
    }

    public void CenterSetup()
    {
        Term.IsCursorVisible = false;
        Center();
        Blocks.Reoffset(RightTextStart, TopLengthStart + 8);
        DrawAll();
        SelectNew();
    }

    public void DrawAll()
    {
        Term.ResetColor();
        Term.Erase();
        SelectNew();
        DrawFrame();
        DrawElements();
        DrawContent();
    }

    private void Center() => (OffsetX, OffsetY) = Centered(Loopover.Usefuls.Convert.GetWindowSize());

    public ResultMessage PlayScramble()
    {
        if (Stats.Count == 0)
            return ResultMessage.None;
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
        var sb = Term.PrepareSB(Term.reset, Term.move, RightTextStart - 2, TopLengthStart - 1);
        int bar = Stats.Count == 0 ? 0 : (TextHeight + 1) * Position / Stats.Count;
        for (int i = 0; i < (TextHeight + 1); i++)
        {
            if (i == bar)
                sb.Append('█');
            else
                sb.Append('│');

            sb.Append(Term.left1).Append(Term.down1);
        }

        Console.Write(sb.ToString());
    }

    public void DrawContent()
    {
        if (Stats.Count == 0)
            return;
        Term.Form(
            Term.reset, Term.move, RightTextStart, TopLengthStart - 1, Term.brightWhite, "ID:    ",
            Term.magenta, $"{Stats.Count - Selected}       ",
            Term.move, RightTextStart, TopLengthStart, Term.brightWhite, "Time:  ",
            Term.green, $"{Selection.time.TotalSeconds:00.000}   ",
            Term.move, RightTextStart, TopLengthStart + 1, Term.brightWhite, "Moves: ",
            Term.red, $"{Selection.numMoves}   ",
            Term.move, RightTextStart, TopLengthStart + 2, Term.brightWhite, "Date:  ",
            Term.yellow, $"{Selection.date:g}     ",
            Term.move, RightTextStart, TopLengthStart + 3, Term.brightWhite, $"Solve (start on {Selection.scramble[(Selection.startY * 3) + Selection.startX]}):"
        );
        MoveReplay.DrawAll();
        Term.Form(Term.move, RightTextStart, TopLengthStart + 7, Term.brightWhite, "Scramble:");
        Blocks.DrawAll();
    }

    public void Move(bool forward)
    {
        if (Stats.Count == 0)
            return;
        (Direction Direction, bool Rotate) = MoveReplay[MoveReplay.Position];
        if (forward && MoveReplay.Move(1))
        {
            Blocks.Play(MoveReplay[MoveReplay.Position].Rotate, MoveReplay[MoveReplay.Position].Direction);
        }
        else if (!forward && MoveReplay.Move(-1) && (Direction != Direction.None))
            Blocks.Play(Rotate, Usefuls.Convert.Reverse(Direction));
    }

    public void ResetMoves()
    {
        if (Stats.Count == 0)
            return;
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
        if (Stats.Count == 0)
            return;
        Blocks.SetTemplateDraw(Selection.scramble, Selection.startX, Selection.startY);
        Blocks.DrawAll();
        MoveReplay = new(Selection.moves, this);
    }

    public void DrawElements()
    {
        var sb = Term.PrepareSB(Term.reset, Term.brightBlack);
        for (int i = Position; (i < (Position + TextHeight)) && (i < Stats.Count) && (i >= 0); i++)
        {
            if (i == Selected)
                sb.Append(Term.brightGreen);
            sb.Append(string.Format(Term.move, LeftTextStart, TopLengthStart + (i - Position)))
                .Append($"{Stats[^(i + 1)].date:d}".PadRight(12))
                .Append($"{Stats[^(i + 1)].time.TotalSeconds:00.000}".PadRight(11))
                .Append($"{Stats[^(i + 1)].numMoves}".PadRight(5))
                .Append(Term.brightBlack);
        }

        Console.Write(sb.ToString());
        DrawLine();
    }

    private void DrawFrame()
    {
        var sb = Term.PrepareSB(Term.reset, Term.move, OffsetX, OffsetY, Term.brightWhite);
        for (int i = 0; i < height; i++)
            sb.Append('█')
                .Append(string.Format(Term.right, width - 2))
                .Append('█')
                .Append(string.Format(Term.move, OffsetX, OffsetY + i + 1));

        sb.Append(string.Format(Term.move, OffsetX + 1, OffsetY))
            .Append('▀', width - 2)
            .Append(string.Format(Term.move, OffsetX + 1, OffsetY + height - 1))
            .Append('▄', width - 2)
            .Append(string.Format(Term.move, LeftTextStart, TopLengthStart - 1))
            .Append("Date        Time       Moves");

        Console.Write(sb.ToString());
        DrawLine();
    }

}
