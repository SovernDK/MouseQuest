using Atlas.Enums;
using Atlas.UI;
using UnityEngine;

public class DevPresenter : IPresenter<DevSystem, DevView>
{
    public DevSystem System { get; set; }
    public DevView View { get; set; }

    public void Initialize()
    {
        View.UpdateItems(System.GetAllItems());
    }

    public void Show()
    {
        View.Show();
    }

    public void Hide()
    {
        View.Hide();
    }

    public void AddCurrency()
    {
        System.Player.InventorySystem.AddCurrency(View.GetCurrencyValue());
    }

    public void AddExp()
    {
        System.Player.AttributeSystem.IncreaseExp(View.GetExpValue());
        System.Player.AttributeSystem.LevelUp();
    }

    public void SetHP()
    {
        System.Player.AttributeSystem.SetAttributeValue(EAttribute.Hitpoints, View.GetHPValue());
        // System.Player.AttributeSystem.Refresh();
    }

    public bool IsViewVisible()
    {
        return View.Visible;
    }
}