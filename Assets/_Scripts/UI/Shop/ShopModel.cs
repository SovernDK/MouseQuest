using System.Collections.Generic;
using Atlas.DB;
using UnityEngine;

public class ShopModel
{
    private ShopItemEntry[] _sellEntries;
    private ShopItemEntry[] _buyEntries;

    public ShopItemEntry[] SellEntries { get => _sellEntries; set => _sellEntries = value; }
    public ShopItemEntry[] BuyEntries { get => _buyEntries; set => _buyEntries = value; }

    public ShopModel()
    {
        
    }

    public void Load(Shop shop, InventorySystem inventory = null)
    {
        List<ShopItemEntry> sell = new List<ShopItemEntry>();
        // foreach(InventorySlot slot in inventory.InventoryModel.Slots)
        // {
        //     Item item = Database.Instance.GetItem(slot.ItemId);
        //     if(item.id == 0) continue;
            
        //     sell.Add(new ShopItemEntry() {
        //         Id = slot.Id,
        //         ItemId = slot.ItemId,
        //         Cost = item.cost,
        //         Value = Mathf.RoundToInt(item.cost * .1f),
        //         Amount = slot.Amount,
        //     });
        // }

        List<ShopItemEntry> buy = new List<ShopItemEntry>();
        for(int i = 0; i < shop.itemsOnSale.Count; i++)
        {
            Item item = Database.Instance.GetItem(shop.itemsOnSale[i].item.name);
            buy.Add(new ShopItemEntry() {
                Id = i,
                ItemId = item.name,
                Cost = item.cost,
                Amount = -1,
            });
        }

        _sellEntries = sell.ToArray();
        _buyEntries = buy.ToArray();
    }

    public void UpdateInventory(InventorySystem inventory)
    {
        List<ShopItemEntry> sell = new List<ShopItemEntry>();
        foreach(InventorySlot slot in inventory.InventoryModel.Slots)
        {
            Item item = Database.Instance.GetItem(slot.ItemId);
            if(item.id == 0) continue;

            sell.Add(new ShopItemEntry() {
                Id = slot.Id,
                ItemId = slot.ItemId,
                Cost = item.cost,
                Value = Mathf.RoundToInt(item.cost * .1f),
                Amount = slot.Amount,
            });
        }

        _sellEntries = sell.ToArray();
    }
}

public class ShopItemEntry
{
    public int Id { get; set; }
    public string ItemId { get; set; }
    public int Cost { get; set; }
    public int Value { get; set; }
    public int Amount { get; set; }
}
