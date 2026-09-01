using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentModel 
{
    [SerializeField]
    private EquipmentSlot[] _slots;
    public EquipmentSlot[] Slots { get => _slots; set => _slots = value; }

    public EquipmentModel()
    {
        _slots = new EquipmentSlot[Enum.GetValues(typeof(EEquipmentSlot)).Length];
        for(int i = 0; i < Enum.GetValues(typeof(EEquipmentSlot)).Length; i++)
        {
            _slots[i] = new EquipmentSlot(i, "item_empty");
        }
    }

    public void LoadModelValues(EquipmentModel data)
    {
        _slots = data.Slots;
    }

    public bool Set(int cellId, string itemId)
    {
        Slots[cellId].ItemId = itemId;
        return true;
    }

    public bool Add(string itemId)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(Slots[i].ItemId.Equals("item_empty"))
            {
                Slots[i].ItemId = itemId;
                return true;
            }
        }
        return false;
    }

    public bool Remove(int cellId)
    {
        Slots[cellId].ItemId = "item_empty";
        return true;
    }

    public string Get(int cellId)
    {
        return Slots[cellId].ItemId;
    }

    public string Get(EEquipmentSlot cellId)
    {
        return Slots[(int) cellId].ItemId;
    }

    public string[] GetAll()
    {
        List<string> result = new List<string>();
        foreach(EquipmentSlot slot in _slots)
        {
            result.Add(slot.ItemId);
        }
        return result.ToArray();
    }
}

[Serializable]
public class EquipmentSlot
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _itemId;

    public int Id { get => _id; set => _id = value; }
    public string ItemId { get => _itemId; set => _itemId = value; }

    public EquipmentSlot(int id, string itemId)
    {
        Id = id;
        ItemId = itemId;
    }
}

public enum EEquipmentSlot
{
    Weapon, Armor, Other
}