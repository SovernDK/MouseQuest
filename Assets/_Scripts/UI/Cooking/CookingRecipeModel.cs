using System.Collections.Generic;
using Atlas.DB;

public class CookingRecipeModel
{
    private List<CookingRecipeEntry> _entries;
    private int count = 3;

    public List<CookingRecipeEntry> Entries { get => _entries; set => _entries = value; }

    public CookingRecipeModel()
    {
        _entries = new List<CookingRecipeEntry>();
        for(int i = 0; i < count; i++)
        {
            _entries.Add(new CookingRecipeEntry() { Id = i, ItemId = "item_empty" });
        }
    }

    public bool TryAddComponent(string itemId, out CookingRecipeEntry found)
    {
        foreach(CookingRecipeEntry entry in _entries)
        {
            if(entry.ItemId.Equals("item_empty")) 
            {
                entry.ItemId = itemId;
                found = entry;
                return true;
            }
        }

        found = null;
        return false;
    }

    public string Get(int cellId)
    {
        return _entries[cellId].ItemId;
    }

    public void Remove(int cellId)
    {
        _entries[cellId].ItemId = "item_empty";
    }

    public void Clear()
    {
        foreach(CookingRecipeEntry entry in _entries)
        {
            entry.ItemId = "item_empty";
        }
    }

    public string[] GetRecipeIds()
    {
        List<string> ids = new List<string>();
        foreach(CookingRecipeEntry entry in _entries)
        {
            ids.Add(entry.ItemId);
        }

        return ids.ToArray();
    }
}

public class CookingRecipeEntry
{
    public int Id { get; set; }
    public string ItemId { get; set; } 
}