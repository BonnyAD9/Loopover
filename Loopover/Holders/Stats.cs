using Loopover.UIs;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Loopover.Holders;

class Stats
{
    public DateTime TimeStart { get; private set; }
    public TimeSpan LastTime { get; private set; }
    public TimeSpan Time => Ingame ? DateTime.Now - TimeStart : LastTime;

    private readonly StatBuilder stat;
    public int Moves => stat.NumMoves;

    public Stat Best { get; private set; }

    private List<Stat> history = new();
    public Stat this[int i]
    {
        get => history[i];
        private set => history[i] = value;
    }

    public int Count => history.Count;

    private bool ingame = false;
    public bool Ingame
    {
        get => ingame;
        set
        {
            if (value == ingame)
                return;
            ingame = value;
            if (ingame)
                TimeStart = DateTime.Now;
            else
                LastTime = DateTime.Now - TimeStart;
        }
    }

    public Stats()
    {
        TimeStart = DateTime.Now;
        LastTime = new(0, 0, 0);
        Best = Stat.Invalid;
        stat = new();
    }

    public void SaveTime()
    {
        Stat st = stat.ToStat(LastTime);
        if (Best.IsInvalid() || (st.time < Best.time))
            Best = st;
        history.Add(st);
    }

    public void NewStat(Blocks b)
    {
        stat.Reset(b.Copy());
    }

    public void SetStart(int x, int y)
    {
        stat.SetStart(x, y);
    }

    public void Move(Direction direction, bool rotate)
    {
        stat.Move(direction, rotate);
    }

    private static readonly string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string folder = Path.Combine(appdata, "Loopover");
    private static readonly string file = Path.Combine(folder, "save.json");

    public bool SaveToJson()
    {
        try
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.WriteAllText(file, JsonConvert.SerializeObject((Best, history), new JsonSerializerSettings() { Formatting = Formatting.Indented }));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void LoadFromJson()
    {
        if (!File.Exists(file))
            return;
        try
        {
            (Best, history) = JsonConvert.DeserializeObject<(Stat, List<Stat>)>(File.ReadAllText(file));
        }
        catch
        {
            (history, Best) = (new(), Stat.Invalid);
        }
    }
}
