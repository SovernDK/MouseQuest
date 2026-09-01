using System.Collections.Generic;
using Atlas.UI;
using Atlas.DB;
// using DB;
// ;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

public class DevView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Transform _devItemContent;
    [SerializeField]
    private Transform _devPermanentItemContent;
    [SerializeField]
    private GameObject _devItemPrefab;

    [SerializeField] [Title("Currency")]
    private TMP_InputField _currencyInputField;
    [SerializeField] [Title("Exp")] 
    private TMP_InputField _expInputField;
    [SerializeField] [Title("HP")] 
    private TMP_InputField _hpInputField;

    [Inject]
    private DevPresenter Presenter { get; set; }

    private List<DevItemRow> _items;
    private List<DevItemRow> _permanentItems;

    #region IView
    public string ViewName => "Dev";
    public bool Visible => _content.gameObject.activeSelf;

    #endregion

    #region IView
    public void Initialize()
    {
        _items = new List<DevItemRow>();
        _permanentItems = new List<DevItemRow>();
        Presenter.View = this;
    }
  
    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }
    
    public void Show()
    {
        _content.gameObject.SetActive(true);
    }
    #endregion

    public int GetCurrencyValue()
    {
        return int.Parse(_currencyInputField.text);
    }

    public float GetExpValue()
    {
        return float.Parse(_expInputField.text);
    }

    public int GetHPValue()
    {
        return int.Parse(_hpInputField.text);
    }

    public void UpdateItems(Item[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(i < _items.Count)
            {
                _items[i].ApplyItem(items[i]);
            }
            else
            {
                GameObject devItemClone = Instantiate(_devItemPrefab, _devItemContent);
                _items.Add(devItemClone.GetComponent<DevItemRow>());
                _items[i].Initialize();
                _items[i].ApplyItem(items[i]);
                // _items[i].OnClicked.AddListener(itemId => { Presenter.System.InventorySystem.AddItem(itemId); });
            }
        }
    }
}
