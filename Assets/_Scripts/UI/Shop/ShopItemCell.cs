using Atlas.DB;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemCell : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _amountLabel;
    [SerializeField]
    private TMP_Text _costLabel;
    [SerializeField]
    private Button _button;

    public UnityEvent<int> OnClicked { get; set; }
    public int CellId { get; set; }
    public string ItemId { get; set; }

    public void Initialize(int cellId)
    {
        CellId = cellId;

        OnClicked = new UnityEvent<int>();
        _button.onClick.AddListener(() => { OnClicked.Invoke(CellId); });
    }

    public void ApplyItem(Item item, int amount, int cost)
    {
        if(item.id.Equals("item_empty")) _button.interactable = false;
        else _button.interactable = true;
        
        ItemId = item.name;
        
        if(LocalizationManager.TryGetTranslation(item.name, out string translation))
            _nameLabel.text = translation;
        else 
            _nameLabel.text = item.name;

        if(amount == -1) _amountLabel.enabled = false;
        else
        {
            _amountLabel.enabled = true;
            _amountLabel.text = "x " + amount.ToString();
        } 
        
        _costLabel.text = cost.ToString();
    }
}
