using ModelShark;
using TMPro;
using UnityEngine;
using I2.Loc;
using CharacterSheet;
using Zenject;
using Atlas.Utility;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AttributeRow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _valueLabel;
    [SerializeField]
    private TMP_Text _maxValueLabel;
    [SerializeField]
    private TooltipTrigger _tooltipTrigger;
    
    [Inject]
    private Config _config;

    public int CellId { get; set; }

    public void Initialize(int cellId)
    {
        CellId = cellId;
    }

    public void ApplyAttribute(Attribute attribute, string name, string description)
    {
        _nameLabel.text = LocalizationManager.GetTranslation(name);

        if(attribute.Value != attribute.BaseValue)
            _valueLabel.text = (attribute.Value > attribute.BaseValue) ? $"<color=#{_config.AttributePositive.ToHexString()}>{attribute.Value}" : $"<color=#{_config.AttributeNegative.ToHexString()}>{attribute.Value}";
        else _valueLabel.text = $"{attribute.Value}";

        if(attribute.VariableAttribute)
        {   
            _maxValueLabel.gameObject.SetActive(true);
            _maxValueLabel.text = $" / {attribute.MaxValue}";
        }
        else
        {
            _maxValueLabel.gameObject.SetActive(false);
        }

        _tooltipTrigger.SetText("Name", LocalizationManager.GetTranslation(name));
        _tooltipTrigger.SetText("Description", LocalizationManager.GetTranslation(description));

        string modifiers = " ";

        foreach(KeyValuePair<string, AttributeModifier> modifier in attribute.Modifiers)
        {
            string sign = (modifier.Value.Value >= 0) ? $"<color=#{_config.AttributePositive.ToHexString()}> +" : $"<color=#{_config.AttributeNegative.ToHexString()}> ";
            (string, string) keys = Util.GetModifiersLocalization(modifier.Key);

            string modifierTypeLocalization = LocalizationManager.GetTranslation(keys.Item1);
            string modifierKeyLocalization = LocalizationManager.GetTranslation(keys.Item2);
            modifiers += $"({modifierTypeLocalization}) {modifierKeyLocalization} {sign} {modifier.Value.Value} <color=#633431>\n";
        }

        if(!modifiers.Trim().Equals(""))
        {
            _tooltipTrigger.TurnSectionOn("Modifiers");
            _tooltipTrigger.SetText("Modifiers", modifiers);
        }
        else 
            _tooltipTrigger.TurnSectionOff("Modifiers");
        
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, AttributeRow>
    {
    }
}
