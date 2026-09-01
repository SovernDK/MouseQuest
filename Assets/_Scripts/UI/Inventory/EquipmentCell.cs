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

public class EquipmentCell : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TooltipTrigger _tooltipTrigger;

    [Inject]
    private Gamemaster _gm;
    [Inject]
    private Config _config;

    private Color _normal;
    private Color _positive;
    private Color _negative;

    public UnityEvent<int> OnClicked { get; set; }
    public int CellId { get; set;}
    public int ItemId { get; set;}

    public void Initialize(int cellId, Item item)
    {
        CellId = cellId;
        ItemId = item.id;

        OnClicked = new UnityEvent<int>();
        _button.onClick.AddListener(() => { OnClicked.Invoke(CellId); });

        _nameLabel.text = item.name;

        _positive = _config.AttributePositive;
        _negative = _config.AttributeNegative;
        _normal = _config.AttributeNormal;
    }

    public void ApplyItem(Item item)
    {
        ItemId = item.id;

        if(item.icon != null)
        {
            _icon.sprite = item.icon;
        }

        if(ItemId != 0)
            _nameLabel.text = LocalizationManager.GetTranslation(item.name);
        else 
            _nameLabel.text = "-";//LocalizationManager.GetTranslation("slot");

        // if(ItemId == 0) 
        // {
        //     _button.interactable = false;
        //     _tooltipTrigger.enabled = false;
        //     return;
        // }
        // else 
        // {
        //     _button.interactable = true;
        //     _tooltipTrigger.enabled = true;
        // }

        _tooltipTrigger.SetImage("Icon", item.icon);
        _tooltipTrigger.SetText("Name", LocalizationManager.GetTranslation(item.name));
        _tooltipTrigger.SetText("Type", LocalizationManager.GetTranslation($"item_type_{item.itemType.ToString().ToLower()}"));
        _tooltipTrigger.SetText("Description", LocalizationManager.GetTranslation(item.description));

        string modifiers = " ";

        for(int i = 0; i < Enum.GetValues(typeof(EAttribute)).Length; i++)
        {
            if(i < item.modifiers.Count)
            {
                _tooltipTrigger.TurnSectionOn(i.ToString());

                string sign = (item.modifiers[i].value >= 0) ? $"<color=#{_positive.ToHexString()}> +" : $"<color=#{_negative.ToHexString()}> ";
                string attributeName = LocalizationManager.GetTranslation(Database.Instance.GetAttribute((int) item.modifiers[i].id).name);
                string attributeShortcut = LocalizationManager.GetTranslation(Database.Instance.GetAttribute((int) item.modifiers[i].id).shortcut);
                modifiers += attributeName + "(" + attributeShortcut + ")" + sign + item.modifiers[i].value + $"<color=#{_normal.ToHexString()}>\n";

                _tooltipTrigger.SetText(i.ToString(), $"{attributeShortcut} {sign}{item.modifiers[i].value}");
            }
            else
            {
                _tooltipTrigger.TurnSectionOff(i.ToString());
            }
        }
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, EquipmentCell>
    {
    }
}
