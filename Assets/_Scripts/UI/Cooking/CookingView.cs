using System.Collections.Generic;
using Atlas.DB;
using Atlas.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class CookingView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    [SerializeField] [FoldoutGroup("Inventory")]
    private GameObject _inventoryCellPrefab;
    [SerializeField] [FoldoutGroup("Inventory")]
    private Transform _inventoryContent;
    [SerializeField] [FoldoutGroup("Recipe")]
    private List<CookingComponentCell> _recipeComponents;
    [SerializeField] [FoldoutGroup("Benefits")]
    private GameObject _benefitsCellPrefab;
    [SerializeField] [FoldoutGroup("Benefits")]
    private Transform _benefitsContent;
    
    [Inject]
    private CookingPresenter Presenter { get; set; }

    public string ViewName => "Cooking";
    public bool Visible => _content.gameObject.activeSelf;

    public void Initialize()
    {
        Presenter.View = this;

        for(int i = 0; i < _recipeComponents.Count; i++)
        {
            _recipeComponents[i].Initialize(i);
            _recipeComponents[i].OnClicked.AddListener(Presenter.PutComponentOut);
        }
    }

    public void ApplyInventory(InventorySlot[] components)
    {
        //Rework to enabling disabling!!!!!!
        foreach(Transform child in _inventoryContent.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < components.Length; i++)
        {
            int cellId = components[i].Id;

            GameObject cellClone = Instantiate(_inventoryCellPrefab, _inventoryContent);
            cellClone.GetComponent<InventoryCell>().Initialize(components[i].Id);
            // cellClone.GetComponent<InventoryCell>().ApplyItem(Database.GetItem(components[i].ItemId), components[i].Amount);
            cellClone.GetComponent<InventoryCell>().Connect(() => { Presenter.System.PutComponentIn(cellId); });
            // cellClone.GetComponent<InventoryCell>().OnClicked.AddListener(Presenter.System.PutComponentIn);
        }
    }

    public void ApplyRecipe(List<CookingRecipeEntry> components)
    {
        for(int i = 0; i < components.Count; i++)
        {
            Item item = Database.Instance.GetItem(components[i].ItemId);
            _recipeComponents[i].ApplyItem(item);
        }
    }

    public void ApplyBenefits(List<AttributeModifier> benefits)
    {
        //Rework to enabling disabling!!!!!!
        foreach(Transform child in _benefitsContent.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < benefits.Count; i++)
        {
            GameObject go = Instantiate(_benefitsCellPrefab, _benefitsContent);
            go.GetComponent<CookingBenefitCell>().Initialize(i);
            go.GetComponent<CookingBenefitCell>().ApplyBenefit(benefits[i]);
        }
    }

    public void Show()
    {
        _content.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }
}
