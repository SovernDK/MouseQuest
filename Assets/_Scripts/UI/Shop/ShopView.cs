using System.Collections.Generic;
using Atlas.DB;
using Atlas.UI;
using DB;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

public class ShopView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private TMP_Text _currency; 
    [SerializeField]
    private GameObject _shopItemCellPrefab;
    [SerializeField] [FoldoutGroup("Sell")]
    private Transform _sellItemContent;
    private List<ShopItemCell> _sellItems;
    [SerializeField] [FoldoutGroup("Buy")]
    private Transform _butyItemContent;
    private List<ShopItemCell> _buyItems;

    public string ViewName => "Shop";
    public bool Visible => _content.gameObject.activeSelf;

    [Inject]
    private ShopPresenter Presenter { get; set; }

    private void Awake() 
    {
        Initialize();
    }

    public void Initialize()
    {
        Presenter.View = this;

        _sellItems = new List<ShopItemCell>();
        _buyItems = new List<ShopItemCell>();
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }

    public void Show()
    {
        _content.gameObject.SetActive(true);
    }

    public void ApplySellItems(ShopItemEntry[] slots)
    {
        // for(int i = 0; i < slots.Length; i++)
        // {
        //     if(i < _sellItems.Count)
        //     {
        //         _sellItems[i].ApplyItem(Depot.Items[slots[i].ItemId], slots[i].Amount, slots[i].Value);
        //     }
        //     else
        //     {
        //         GameObject cellClone = Instantiate(_shopItemCellPrefab, _sellItemContent);
        //         _sellItems.Add(cellClone.GetComponent<ShopItemCell>());
        //         _sellItems[i].Initialize(slots[i].Id);
        //         _sellItems[i].ApplyItem(Depot.Items[slots[i].ItemId], slots[i].Amount, slots[i].Value);

        //         _sellItems[i].OnClicked.AddListener(Presenter.SellItem);
        //     }
        // }
    }

    public void ApplyCurrency(int value)
    {
        _currency.text = $"{value} Coin";
    }

    public void ApplyBuyItems(ShopItemEntry[] slots)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < _buyItems.Count)
            {
                _buyItems[i].ApplyItem(Database.Instance.GetItem(slots[i].ItemId), -1, slots[i].Cost);
            }
            else
            {
                GameObject cellClone = Instantiate(_shopItemCellPrefab, _butyItemContent);
                _buyItems.Add(cellClone.GetComponent<ShopItemCell>());
                _buyItems[i].Initialize(slots[i].Id);
                _buyItems[i].ApplyItem(Database.Instance.GetItem(slots[i].ItemId), -1, slots[i].Cost);

                _buyItems[i].OnClicked.AddListener(Presenter.BuyItem);
            }
        }
    }
}
