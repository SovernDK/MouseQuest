using Atlas.UI;

public class InventoryPresenter : IPresenter<InventorySystem, InventoryView>
{
    public InventorySystem System { get; set; }
    public InventoryView View { get; set; }

    public void InitializeInventory()
    {
        View.InventorySize = System.InventorySize;
        View.ApplyInventory(System.InventoryModel.Slots);
        // View.InitializeEquipmentSlots();
        // View.ApplyEquipment(System.EquipmentModel.Slots);
        View.ApplyCurrency(0);
    }

    public void UpdateInventory(InventorySlot[] slots)
    {
        View.ApplyInventory(slots);
    }

    public void UpdateCurrency(int value)
    {
        View.ApplyCurrency(value);
    }

    public void UpdateEquipment(EquipmentSlot[] slots)
    {
        View.ApplyEquipment(slots);
    }

    public void EnableInventory(bool enabled)
    {
        View.EnableInventory(enabled);
    }

    public void Show()
    {
        View.Show();
    }

    public void Hide()
    {
        View.Hide();
    }

    public void Toogle()
    {
        if(View.Visible)
        {
            View.Hide();
        }
        else
        {
            View.Show();
        }
    }
}
