using System.Collections.Generic;
using Atlas.DB;
using Atlas.UI;

public class CookingPresenter : IPresenter<CookingSystem, CookingView>
{
    public CookingSystem System { get; set; }
    public CookingView View { get; set; }

    public void Show()
    {
        View.Show();
    }

    public void Hide()
    {
        View.Hide();
    }

    public void ApplyInventory(InventorySlot[] components)
    {
        View.ApplyInventory(components);
    }

    public void ApplyRecipe(List<CookingRecipeEntry> components)
    {
        View.ApplyRecipe(components);
    }

    public void ApplyBenefits(List<AttributeModifier> benefits)
    {
        View.ApplyBenefits(benefits);
    }

    public void PutComponentOut(int cellId)
    {
        System.PutComponentOut(cellId);
    }
}