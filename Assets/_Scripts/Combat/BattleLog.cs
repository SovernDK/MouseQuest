using System.Collections.Generic;
using UnityEngine.Events;

public class BattleLog 
{
    public Queue<BattleLogEntry> Entries { get; set; }
    public UnityEvent<BattleLogEntry> NewEntryAdded { get; set; }

    public BattleLog()
    {
        Entries = new Queue<BattleLogEntry>();
        NewEntryAdded = new UnityEvent<BattleLogEntry>();
    }

    public void NewLogEntry(BattleLogEntry entry)
    {
        Entries.Enqueue(entry);
        NewEntryAdded.Invoke(entry);
    }
}

public abstract class BattleLogEntry
{
    public abstract string GetLogValue();
}