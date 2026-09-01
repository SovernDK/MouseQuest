using System.Collections.Generic;
using UnityEngine;
using Atlas.DB;
using System.Linq;

public class SpellModel : IModel
{
    private SpellEntry[] _spells;
    private int _spellCount = 20;

    public SpellEntry[] Spells { get => _spells; set => _spells = value; }
    public SpellEntry[] UnlockedSpells { get => _spells.ToList().FindAll(spell => spell.Unlocked == true).ToArray(); }

    public SpellModel()
    {
        List<SpellEntry> s = new List<SpellEntry>();

        foreach (Spell spell in Database.Instance.GetAllSpells())
        {
            s.Add(new SpellEntry(spell.name) { MaxAmount = spell.maxPrepared });
        }

        _spellCount = s.Count;
        _spells = s.ToArray();
    }

    public bool Unlock(string spellId)
    {
        foreach(SpellEntry e in _spells)
        {
            if(e.Id.Equals(spellId))
            {
                e.Unlocked = true;
                return true;
            }
        }

        return false;
    }

    public void FillAllSpells()
    {
        foreach(SpellEntry e in _spells)
        {
            e.Amount = e.MaxAmount;
        }
    }

    public SpellEntry Get(string id)
    {
        return _spells.ToList().Find(spell => spell.Id.Equals(id));
    }

    public string[] GetUnlockedSpells()
    {
        List<string> result = new List<string>();
        foreach(SpellEntry e in _spells)
        {
            if(!e.Id.Equals("spell_none") && e.Unlocked) result.Add(e.Id);
        }

        return result.ToArray();
    }

    public void DecreaseSpellAmount(string id, int amount)
    {
        SpellEntry entry = _spells.ToList().Find(spell => spell.Id.Equals(id));
        entry.Amount = Mathf.Clamp(entry.Amount - amount, 0, entry.MaxAmount);
    }

    public void IncreaseSpellAmount(string id, int amount)
    {
        SpellEntry entry = _spells.ToList().Find(spell => spell.Id.Equals(id));
        entry.Amount = Mathf.Clamp(entry.Amount + amount, 0, entry.MaxAmount);
    }

    public bool TryFind(int needle, out SpellEntry found)
    {
        for(int i = 0; i < _spells.Length; i++)
        {
            if(_spells[i].Id.Equals(needle))
            {
                found = _spells[i];
                return true;
            }
        }

        found = null;
        return false;
    }
}

public class SpellEntry
{
    public string Id { get; set; }
    public bool Unlocked { get; set; }
    public int Amount { get; set; }
    public int MaxAmount { get; set; }

    public SpellEntry(string id)
    {
        Id = id;
    }
    public override string ToString()
    {
        return $"SpellEntry: [Id = {Id}, Unlocked = {Unlocked}, Amount = {Amount}]";
    }
}