using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class InventoryModel
{
    [SerializeField]
    private InventorySlot[] _slots;

    [SerializeField]
    private int _size;
    
    private int _currentCurrency;

    public InventorySlot[] Slots { get => _slots; set => _slots = value; }
    public int CurrentCurrency { get => _currentCurrency; set => _currentCurrency = value; }
    public int MaxCurrency { get; set; }

    public InventoryModel(int inventorySize)
    {
        _size = inventorySize;

        _slots = new InventorySlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            _slots[i] = new InventorySlot(i, "item_empty");
        }

        MaxCurrency = 9999;
    }

    public void LoadModelValues(InventoryModel data)
    {
        _slots = data.Slots;
        _currentCurrency = data.CurrentCurrency;
    }

    // public bool Add(int itemId, int amount = 1)
    // {
    //     if(itemId == 0) return false;

    //     if(TryFind(itemId, out InventorySlot slot))
    //     {         
    //         slot.ItemId = itemId;
    //         slot.Amount = Mathf.Clamp(slot.Amount + amount, 0, 99);
    //         return true;
    //     }
    //     else
    //     {
    //         foreach(InventorySlot s in _slots)
    //         {
    //             if(s.ItemId == 0)
    //             {
    //                 s.ItemId = itemId;
    //                 s.Amount = Mathf.Clamp(amount, 0, 99);
    //                 return true;
    //             }
    //         }
    //     }
        
    //     return false;
    // }

    public bool Add(string itemId, int amount = 1)
    {
        if(itemId.Equals("item_empty")) return false;

        if(TryFind(itemId, out InventorySlot slot))
        {         
            slot.ItemId = itemId;
            slot.Amount = Mathf.Clamp(slot.Amount + amount, 0, 99);
            return true;
        }
        else
        {
            foreach(InventorySlot s in _slots)
            {
                if(s.ItemId.Equals("item_empty"))
                {
                    s.ItemId = itemId;
                    s.Amount = Mathf.Clamp(amount, 0, 99);
                    return true;
                }
            }
        }
        
        return false;
    }

    public bool Set(int cellId, string itemName, int amount)
    {
        Slots[cellId].ItemId = itemName;
        Slots[cellId].Amount = amount;
        return true;
    }

    public bool Set(string itemName, int amount)
    {
        if(TryFind(itemName, out InventorySlot slot))
        {         
            slot.ItemId = itemName;
            slot.Amount = Mathf.Clamp(amount, 0, 99);
            return true;
        }

        return false;
    }

    public bool Remove(int cellId)
    {
        Slots[cellId].ItemId = "item_empty";
        // SortSlotsByItem();
        return true;
    }

    public bool Take(int cellId, int amount)
    {
        Slots[cellId].Amount = Mathf.Clamp(Slots[cellId].Amount - amount, 0, 99);
        if(Slots[cellId].Amount == 0)
        {
            Slots[cellId].ItemId = "item_empty";
        }

        // SortSlotsByItem();
        return true;
    }

    public string Get(int cellId)
    {
        return Slots[cellId].ItemId;
    }

    public bool TryFind(string needle, out InventorySlot found)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(Slots[i].ItemId == needle)
            {
                found = Slots[i];
                return true;
            }
        }

        found = null;
        return false;
    }

    private void SortSlotsByItem()
    {
        Slots = Slots
            .OrderByDescending(item => !item.ItemId.Equals("item_empty"))
            .ThenBy(item => item.ItemId)
            .ToArray();
    }
}

[Serializable]
public class InventorySlot
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _itemId;
    [SerializeField]
    private int _amount;
    [SerializeField]
    private bool _hidden;

    public int Id { get => _id; set => _id = value; }
    public string ItemId { get => _itemId; set => _itemId = value; }
    public int Amount { get => _amount; set => _amount = value; }
    public bool Hidden { get => _hidden; set => _hidden = value; }

    public InventorySlot(int id, string itemId, int amount = 0)
    {
        Id = id;
        ItemId = itemId;
        Amount = amount;
    }

    public bool IsEmpty()
    {
        return ItemId.Equals("item_empty");
    }
}
