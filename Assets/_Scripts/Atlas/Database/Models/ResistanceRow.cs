using ModelShark;
using TMPro;
using UnityEngine;
using I2.Loc;
using CharacterSheet;
using Zenject;
using Atlas.Utility;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ResistanceRow : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _valueLabel;
    [SerializeField]
    private TooltipTrigger _tooltipTrigger;
    
    [Inject]
    private Config _config;

    public int CellId { get; set; }

    public void Initialize(int cellId)
    {
        CellId = cellId;
    }

    public void ApplyResistance(Resistance resistance, Sprite icon, string name, string description)
    {
        _icon.sprite = icon;

        if(resistance.Value != resistance.BaseValue)
            _valueLabel.text = (resistance.Value > resistance.BaseValue) ? $"<color=#{_config.AttributePositive.ToHexString()}>{resistance.Value}" : $"<color=#{_config.AttributeNegative.ToHexString()}>{resistance.Value}";
        else _valueLabel.text = $"{resistance.Value}";

        _tooltipTrigger.SetText("Name", LocalizationManager.GetTranslation(name));
        _tooltipTrigger.SetText("Description", LocalizationManager.GetTranslation(description));


        string modifiers = " ";

        foreach(KeyValuePair<string, ResistanceModifier> modifier in resistance.Modifiers)
        {
            string sign = (modifier.Value.Value >= 0) ? $"<color=#{_config.AttributePositive.ToHexString()}> +" : $"<color=#{_config.AttributeNegative.ToHexString()}> ";
            (string, string) keys = Util.GetModifiersLocalization(modifier.Key);

            string modifierTypeLocalization = LocalizationManager.GetTranslation(keys.Item1);
            string modifierKeyLocalization = LocalizationManager.GetTranslation(keys.Item2);
            modifiers += $"({modifierTypeLocalization}) {modifierKeyLocalization} {sign} {modifier.Value.Value} <color=#633431>\n";
        }

        _tooltipTrigger.SetText("Modifiers", modifiers);
    }

    public class Factory : PlaceholderFactory<Object, ResistanceRow> { }
}
