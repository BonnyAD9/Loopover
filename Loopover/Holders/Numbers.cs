namespace Loopover.Holders;

static class Numbers
{
    public static string[] Selected(int num) => num switch
    {
        1 => OneSel,
        2 => TwoSel,
        3 => ThreeSel,
        4 => FourSel,
        5 => FiveSel,
        6 => SixSel,
        7 => SevenSel,
        8 => EightSel,
        9 => NineSel,
        _ => OneSel
    };

    public static string[] Unselected(int num) => num switch
    {
        1 => One,
        2 => Two,
        3 => Three,
        4 => Four,
        5 => Five,
        6 => Six,
        7 => Seven,
        8 => Eight,
        9 => Nine,
        _ => One
    };

    public static string[] OneSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█   ▄█    █",
        "█    █    █",
        "█    █    █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] TwoSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  ▄▀▀▀▄  █",
        "█    ▄▀   █",
        "█  ▄█▄▄▄  █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] ThreeSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  ▄▀▀▀▄  █",
        "█     ▀▄  █",
        "█  ▀▄▄▄▀  █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] FourSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█   █     █",
        "█   █▄█   █",
        "█     █   █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] FiveSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  █▀▀▀▀  █",
        "█  █▄▀▀▄  █",
        "█  ▄▄▄▄▀  █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] SixSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█   ▄▀    █",
        "█   █▀▀▄  █",
        "█   ▀▄▄▀  █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] SevenSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  ▀▀▀▀█  █",
        "█     █   █",
        "█    █    █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] EightSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  ▄▀▀▀▄  █",
        "█  ▄▀▀▀▄  █",
        "█  ▀▄▄▄▀  █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] NineSel { get; } = new string[]
    {
        "█▀▀▀▀▀▀▀▀▀█",
        "█  ▄▀▀▄   █",
        "█   ▀▀█   █",
        "█   ▄▄▀   █",
        "█▄▄▄▄▄▄▄▄▄█"
    };

    public static string[] One { get; } = new string[]
    {
        "           ",
        "    ▄█     ",
        "     █     ",
        "     █     ",
        "           "
    };

    public static string[] Two { get; } = new string[]
    {
        "           ",
        "   ▄▀▀▀▄   ",
        "     ▄▀    ",
        "   ▄█▄▄▄   ",
        "           "
    };

    public static string[] Three { get; } = new string[]
    {
        "           ",
        "   ▄▀▀▀▄   ",
        "      ▀▄   ",
        "   ▀▄▄▄▀   ",
        "           "
    };

    public static string[] Four { get; } = new string[]
    {
        "           ",
        "    █      ",
        "    █▄█    ",
        "      █    ",
        "           "
    };

    public static string[] Five { get; } = new string[]
    {
        "           ",
        "   █▀▀▀▀   ",
        "   █▄▀▀▄   ",
        "   ▄▄▄▄▀   ",
        "           "
    };

    public static string[] Six { get; } = new string[]
    {
        "           ",
        "    ▄▀     ",
        "    █▀▀▄   ",
        "    ▀▄▄▀   ",
        "           "
    };

    public static string[] Seven { get; } = new string[]
    {
        "           ",
        "   ▀▀▀▀█   ",
        "      █    ",
        "     █     ",
        "           "
    };

    public static string[] Eight { get; } = new string[]
    {
        "           ",
        "   ▄▀▀▀▄   ",
        "   ▄▀▀▀▄   ",
        "   ▀▄▄▄▀   ",
        "           "
    };

    public static string[] Nine { get; } = new string[]
    {
        "           ",
        "   ▄▀▀▄    ",
        "    ▀▀█    ",
        "    ▄▄▀    ",
        "           "
    };
}
