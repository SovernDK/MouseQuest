using Atlas.Core;
// ;
using Atlas.DB;
using ModelShark;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using I2.Loc;
using Unity.VisualScripting;
using System;
using Zenject;
using Atlas.Utility;
using Atlas.Enums;

public class InventoryCell : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _amountLabel;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TooltipTrigger _tooltipTrigger;

    private Color _normal;
    private Color _positive;
    private Color _negative;

    [Inject]
    private Config Config { get; }

    public UnityEvent<int> OnClicked { get; set; }
    public int CellId { get; set; }
    public string ItemId { get; set; }
    public Button Button { get => _button; set => _button = value; }

    public void Initialize(int cellId)
    {
        CellId = cellId;

        OnClicked = new UnityEvent<int>();
        GetComponent<RectTransform>().localScale = Vector3.one;

        _positive = Config.AttributePositive;
        _negative = Config.AttributeNegative;
        _normal = Config.AttributeNormal;
    }

    public void Connect(UnityAction call)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(call);
    }

    public void ApplyItem(Item item, int amount)
    {
        ItemId = item.name;

        if(!ItemId.Equals("item_empty"))
            ConsoleProDebug.LogToFilter("Applied item by name " + LocalizationManager.GetTranslation(item.name), "InventorySystem");
        
        _icon.sprite = item.icon;

        if(LocalizationManager.TryGetTranslation(item.name, out string translation))
            _nameLabel.text = translation;
        else 
            _nameLabel.text = item.name;

        if(ItemId.Equals("item_empty")) 
        {
            _button.interactable = false;
            _tooltipTrigger.enabled = false;

            _amountLabel.text = "-";
            return;
        }
        else 
        {
            _button.interactable = true;
            _tooltipTrigger.enabled = true;

            _amountLabel.text = amount.ToString();
        }

        _tooltipTrigger.SetImage("Icon", item.icon);
        _tooltipTrigger.SetText("Name", LocalizationManager.GetTranslation(item.name));
        _tooltipTrigger.SetText("Type", LocalizationManager.GetTranslation($"item_type_{item.itemType.ToString().ToLower()}"));
        _tooltipTrigger.SetText("Description", LocalizationManager.GetTranslation(item.description));

        // string modifiers = " ";

        // for(int i = 0; i < Enum.GetValues(typeof(EAttribute)).Length; i++)
        // {
        //     if(i < item.modifiers.Count)
        //     {
        //         _tooltipTrigger.TurnSectionOn(i.ToString());

        //         string sign = (item.modifiers[i].value >= 0) ? $"<color=#{_positive.ToHexString()}> +" : $"<color=#{_negative.ToHexString()}> ";
        //         string attributeName = LocalizationManager.GetTranslation(Database.Instance.GetAttribute((int) item.modifiers[i].id).name);
        //         string attributeShortcut = LocalizationManager.GetTranslation(Database.Instance.GetAttribute((int) item.modifiers[i].id).shortcut);
        //         modifiers += attributeName + "(" + attributeShortcut + ")" + sign + item.modifiers[i].value + $"<color=#{_normal.ToHexString()}>\n";

        //         _tooltipTrigger.SetText(i.ToString(), $"{attributeShortcut} {sign}{item.modifiers[i].value}");
        //     }
        //     else
        //     {
        //         _tooltipTrigger.TurnSectionOff(i.ToString());
        //     }
        // }
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, InventoryCell>
    {
    }
}
