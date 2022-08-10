using System;
using System.Text;
using Bny.Console;
using Loopover.Holders;

namespace Loopover.UIs;

class Blocks
{
    private readonly int[] board;

    public int TileWidth { get; private set; } = 11;
    public int TileHeight { get; private set; } = 5;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int OffsetX { get; private set; }
    public int OffsetY { get; private set; }

    public int CharWidth => TileWidth * Width;
    public int CharHeight => TileHeight * Height;

    public int SelX { get; set; } = 0;
    public int SelY { get; set; } = 0;
    public int Selected => this[SelX, SelY];

    public delegate string[] GetBlock(int num);
    private readonly GetBlock GetSel = BigSel;
    private readonly GetBlock GetUnsel = BigUnsel;

    public int this[int x, int y]
    {
        get => board[(Width * y) + x];
        private set => board[(Width * y) + x] = value;
    }

    public Blocks(int x, int y) : this(x, y, 0, 0) { }
    public Blocks(int x, int y, int offsetx, int offsety)
    {
        (Width, Height, OffsetX, OffsetY) = (x, y, offsetx, offsety);
        board = new int[x * y];
        for (int i = 0; i < board.Length; i++)
            board[i] = i + 1;
    }
    public Blocks(int x, int y, bool small) : this(x, y, 0, 0, small) { }
    public Blocks(int x, int y, int offsetX, int offsetY, bool small)
    {
        (Width, Height, OffsetX, OffsetY) = (x, y, offsetX, offsetY);
        board = new int[x * y];
        for (int i = 0; i < board.Length; i++)
            board[i] = i + 1;
        if (small)
        {
            (GetSel, GetUnsel) = (SmallSel, SmallUnsel);
            (TileWidth, TileHeight) = (7, 3);
        }
    }

    public void SetTemplateDraw(int[] template, int x, int y)
    {
        template.CopyTo(board, 0);
        SelX = x;
        SelY = y;
        DrawAll();
    }

    public void Center()
    {
        (int width, int height) = Term.GetWindowSize();
        Reoffset((width / 2) - (CharWidth / 2), (height / 2) - (CharHeight / 2));
    }

    public void CenterDraw()
    {
        Center();
        ClearDraw();            
    }

    private static void Clear()
    {
        Term.ResetColor();
        Term.Erase();
    }

    public void ClearDraw()
    {
        Clear();
        DrawAll();
    }

    public void Setup()
    {
        ClearDraw();
        Term.IsCursorVisible = false;
    }

    public void SetupCenter()
    {
        CenterDraw();
        Term.IsCursorVisible = false;
    }

    public bool CheckWin()
    {
        for (int i = 0; i < (board.Length - 1); i++)
        {
            if (board[i] > board[i + 1])
                return false;
        }
        return true;
    }

    private void Scramble()
    {
        (int, int) save = (SelX, SelY);
        int lim = Width * Height * Math.Max(Width, Height) < 20 ? 20 : Width * Height * Math.Max(Width, Height);
        for (int i = 0; i < lim; i++)
        {
            (SelX, SelY) = (Program.random.Next(Width), Program.random.Next(Height));
            RotateSel((Direction)Enum.GetValues(typeof(Direction)).GetValue(Program.random.Next(4)));
        }
        (SelX, SelY) = save;
    }

    public void ScrambleDraw()
    {
        Scramble();
        DrawAll();
    }

    public void DrawAll()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
                DrawAt(x, y, this[x, y] == Selected);
        }
    }

    private void DrawAt(int x, int y, bool selected)
    {
        int num = this[x, y];

        var sb = Term.PrepareSB(
            Term.move, OffsetX + (x * TileWidth), OffsetY + (y * TileHeight),
            GetColor(num), Term.brightWhite
        );

        foreach (string s in selected ? GetSel(num) : GetUnsel(num))
            sb.Append(s).Append(Term.down1).Append(Term.Prepare(Term.left, TileWidth));

        Console.Write(sb.Append(Term.defaultBg).ToString());
    }

    private void MoveSel(int x, int y)
    {
        x %= Width;
        y %= Height;
        SelX = (Width + SelX + x) % Width;
        SelY = (Height + SelY + y) % Height;
    }

    public void MoveSelDraw(int x, int y)
    {
        DrawAt(SelX, SelY, false);
        MoveSel(x, y);
        DrawAt(SelX, SelY, true);
    }

    private void RotateSel(Direction direction)
    {
        int hold;
        switch (direction)
        {
            case Direction.Right:
                hold = this[Width - 1, SelY];
                for (int i = 0; i < Width; i++)
                    (this[i, SelY], hold) = (hold, this[i, SelY]);
                break;
            case Direction.Down:
                hold = this[SelX, Height - 1];
                for (int i = 0; i < Height; i++)
                    (this[SelX, i], hold) = (hold, this[SelX, i]);
                break;
            case Direction.Left:
                hold = this[0, SelY];
                for (int i = Width - 1; i >= 0; i--)
                    (this[i, SelY], hold) = (hold, this[i, SelY]);
                break;
            case Direction.Up:
                hold = this[SelX, 0];
                for (int i = Height - 1; i >= 0; i--)
                    (this[SelX, i], hold) = (hold, this[SelX, i]);
                break;
        }
        return;
    }

    public void RotateSelDraw(Direction direction)
    {
        RotateSel(direction);
        if (((int)direction & 1) == 0)
        {
            MoveSel(((int)direction & 2) == 2 ? 1 : -1, 0);
            for (int x = 0; x < Width; x++)
                DrawAt(x, SelY, x == SelX);
            return;
        }
        MoveSel(0, ((int)direction & 2) == 2 ? 1 : -1);
        for (int y = 0; y < Height; y++)
            DrawAt(SelX, y, y == SelY);
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                MoveSelDraw(-1, 0);
                return;
            case Direction.Right:
                MoveSelDraw(1, 0);
                return;
            case Direction.Up:
                MoveSelDraw(0, -1);
                return;
            case Direction.Down:
                MoveSelDraw(0, 1);
                return;
        }
    }

    public void Play(bool rotate, Direction direction)
    {
        if (rotate)
            RotateSelDraw(direction);
        else
            Move(direction);
    }

    public void Reoffset(int x, int y)
    {
        OffsetX = x;
        OffsetY = y;
    }

    public string GetColor(int num)
    {
        const float scaleDown = 0.85f;
        num--;
        int x = (num % Width) * 255 / (Width - 1);
        int y = (num / Width) * 255 / (Height - 1);
        return string.Format(Term.bg, (byte)((255 - x) * scaleDown), (byte)(y * scaleDown), (byte)(x * scaleDown));
    }

    public static string[] BigSel(int num)
    {
        return Numbers.Selected(num);
    }

    public static string[] BigUnsel(int num)
    {
        return Numbers.Unselected(num);
    }

    public static string[] SmallSel(int num) => num switch
    {
        < 10 => new string[] {
                "█▀▀▀▀▀█",
                $"█  {num}  █",
                "█▄▄▄▄▄█"
            },
        < 100 => new string[] {
                "█▀▀▀▀▀█",
                $"█ {num}  █",
                "█▄▄▄▄▄█"
            },
        < 1000 => new string[] {
                "█▀▀▀▀▀█",
                $"█ {num} █",
                "█▄▄▄▄▄█"
            },
        _ => new string[] {
                "█▀▀▀▀▀█",
                $"█ {num}█",
                "█▄▄▄▄▄█"
            },
    };

    public static string[] SmallUnsel(int num) => num switch
    {
        < 10 => new string[] {
            "       ",
            $"   {num}   ",
            "       "
        },
        < 100 => new string[] {
            "       ",
            $"  {num}   ",
            "       "
        },
        < 1000 => new string[] {
            "       ",
            $"  {num}  ",
            "       "
        },
        _ => new string[] {
            "       ",
            $"  {num} ",
            "       "
        },
    };

    public int[] Copy()
    {
        int[] arr = new int[board.Length];
        board.CopyTo(arr, 0);
        return arr;
    }

}
