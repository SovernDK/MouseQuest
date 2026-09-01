using System.Collections.Generic;
using Atlas.UI;

public class ShopPresenter : IPresenter<ShopSystem, ShopView>
{
    public ShopSystem System { get; set; }
    public ShopView View { get; set; }

    public void Open(ShopItemEntry[] sellEntries, ShopItemEntry[] buyEntries)
    {
        // View.ApplySellItems(sellEntries);
        View.ApplyBuyItems(buyEntries);
        View.ApplyCurrency(System.Player.GetComponent<InventorySystem>().InventoryModel.CurrentCurrency);
        
        View.Show();
    }

    public void Close()
    {
        View.Hide();
    }

    public void ApplySellItems()
    {
        View.ApplySellItems(System.ShopModel.SellEntries);
    }

    public void Update()
    {
        // View.ApplySellItems(System.ShopModel.SellEntries);
        View.ApplyBuyItems(System.ShopModel.BuyEntries);
        View.ApplyCurrency(System.Player.GetComponent<InventorySystem>().InventoryModel.CurrentCurrency);
    }

    public void BuyItem(int cellId)
    {
        System.Buy(cellId);   
    }

    public void SellItem(int cellId)
    {
        System.Sell(cellId);   
    }
}
