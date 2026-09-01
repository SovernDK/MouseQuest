using Atlas.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootRow : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _amountLabel;
    
    private ResourcesSystem _resources;

    public int CellId { get; set; }
    public int ItemId { get; set; }

    public void Initialize(int cellId, ResourcesSystem resources)
    {
        CellId = cellId;
        _resources = resources;
    }

    public void ApplyLoot(Atlas.DB.Item item, int amount)
    {
        ItemId = item.id;

        if(item.icon != null)
        {
            _icon.sprite = item.icon;
        }
        
        // _nameLabel.text = LocalizationManager.GetTranslation(item.name);
        _amountLabel.text = "x" + amount.ToString();
    }

    public void FadeIn()
    {
        _icon.DOFade(1, .6f);
        // _nameLabel.DOFade(1, .75f);
        _amountLabel.DOFade(1, .75f);
    }

    public void Punch()
    {
        _amountLabel.GetComponent<RectTransform>().DOPunchPosition(new Vector3(0, 1, 0), .25f, 1, 1);
        _amountLabel.GetComponent<RectTransform>().DOPunchScale(new Vector3(1, 1, 0), .25f, 1, 1);
    }
}
