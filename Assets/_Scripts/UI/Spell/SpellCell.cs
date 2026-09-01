using I2.Loc;
using Atlas.DB;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Atlas.Utility;
using ModestTree;
using System.Linq;
using ModelShark;

public class SpellCell : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _amountLabel;
    [SerializeField]
    private Image _typeIcon;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TooltipTrigger _tooltipTrigger;

    public int Id { get; set; }
    public string SpellId { get; set; }
    public UnityEvent<string> OnClicked { get; set; }
    public Button Button { get => _button; set => _button = value; }

    public void Initialize(int id)
    {
        Id = id;

        OnClicked = new UnityEvent<string>();
        _button.onClick.AddListener(() => { OnClicked.Invoke(SpellId); });
    }

    public void ApplySpell(SpellEntry entry)
    {
        SpellId = entry.Id;
        Spell spell = Database.Instance.GetSpell(SpellId);

        if(LocalizationManager.TryGetTranslation(spell.name, out string translation))
            _nameLabel.text = translation;
        else 
            _nameLabel.text = spell.name;

        _icon.sprite = spell.icon;
        _typeIcon.sprite = spell.castType == ECastType.Normal ? Config.Instance.castNormal : Config.Instance.castQuick;

        _amountLabel.text = entry.Amount.ToString() + " / " + spell.maxPrepared;

        _tooltipTrigger.SetText("Name", _nameLabel.text);

        if(LocalizationManager.TryGetTranslation(spell.description, out string description))
        {
            _tooltipTrigger.TurnSectionOn("Description");
            _tooltipTrigger.SetText("Description", LocalizationManager.GetTranslation(description));
        }
        else 
            _tooltipTrigger.TurnSectionOff("Description");

    }
}
