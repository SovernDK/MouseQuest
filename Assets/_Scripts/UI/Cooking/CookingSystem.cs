using System.Collections.Generic;
using System.Linq;
using Atlas.DB;
using Atlas.Player;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class CookingSystem : MonoBehaviour
{
    [Inject]
    private CookingPresenter Presenter { get; set; }
    [Inject]
    public PlayerSystem Player { get; }

    public CookingRecipeModel RecipeModel { get; set; }

    public UnityEvent CookingStarted { get; set; }
    public UnityEvent CookingEnded { get; set; }
    public UnityEvent<int> ComponentTaken { get; set; }
    public UnityEvent<string> ComponentReturned { get; set; }
    public UnityEvent<List<AttributeModifier>> Cooked { get; set; }

    public InventorySlot[] Components => Player.InventorySystem.GetItemsByType(EItemType.Component);

    public void Initialize()
    {
        Presenter.System = this;

        RecipeModel = new CookingRecipeModel();

        CookingStarted = new UnityEvent();
        CookingEnded = new UnityEvent();

        ComponentTaken = new UnityEvent<int>();
        ComponentReturned = new UnityEvent<string>();
        Cooked = new UnityEvent<List<AttributeModifier>>();
    }

    public void Open()
    {
        CookingStarted.Invoke();
        
        Refresh();
        Presenter.Show(); 
    }

    public void Close()
    {
        Presenter.Hide();
        ReturnAllComponentsToInventory();
        
        CookingEnded.Invoke();
    }

    public void Refresh()
    {
        Presenter.ApplyInventory(Components);
        Presenter.ApplyRecipe(RecipeModel.Entries);
        Presenter.ApplyBenefits(GetBenefits());
    }

    public void PutComponentIn(int cellId)
    {
        ConsoleProDebug.LogToFilter($"Clicked CellId {cellId}", "CookingSystem");
        // int itemId = Player.InventorySystem.InventoryModel.Get(cellId);
        
        // if(RecipeModel.TryAddComponent(itemId, out CookingRecipeEntry entry))
        // {
        //     ComponentTaken.Invoke(cellId);
        // }
        
        Refresh();
    }

    public void PutComponentOut(int cellId)
    {
        string itemId = RecipeModel.Get(cellId);
        RecipeModel.Remove(cellId);

        ComponentReturned.Invoke(itemId);

        Refresh();
    }

    public void Cook()
    {
        Cooked.Invoke(GetBenefits());
        RecipeModel.Clear();

        Refresh();
    }

    private void ReturnAllComponentsToInventory()
    {
        foreach(CookingRecipeEntry entry in RecipeModel.Entries)
        {
            RecipeModel.Remove(entry.Id);
            string itemId = RecipeModel.Get(entry.Id);

            ComponentReturned.Invoke(itemId);
        }

        Refresh();
    }

    private List<AttributeModifier> GetBenefits()
    {
        List<AttributeModifier> result = new List<AttributeModifier>();

        foreach (CookingRecipeEntry entry in RecipeModel.Entries)
        {
            Item item = Database.Instance.GetItem(entry.ItemId);
            result.AddRange(item.cookingBenefits);
        }

        List<AttributeModifier> groupedBenefits = result
            .GroupBy(modifier => modifier.id)
            .Select(group => new AttributeModifier
            {
                id = group.Key,
                value = group.Sum(modifier => modifier.value)
            })
            .ToList();

        return groupedBenefits;
    }
}
