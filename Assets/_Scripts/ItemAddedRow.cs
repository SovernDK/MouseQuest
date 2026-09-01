using System.Collections;
using Atlas.DB;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAddedRow : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _amountLabel;

    public void ApplyItem(Item item, int amount)
    {
        _icon.sprite = item.icon;
        _amountLabel.text = amount.ToString();
    }

    public IEnumerator Show(Item item, int amount)
    { 
        _icon.DOFade(0, 0);
        _amountLabel.DOFade(0, 0);

        _icon.DOFade(1, 1);
        yield return _amountLabel.DOFade(1, 1);

        _icon.sprite = item.icon;
        _amountLabel.text = "+" + amount.ToString();

        yield return new WaitForSeconds(.5f);

        _icon.DOFade(0, 1);
        yield return _amountLabel.DOFade(0, 1);
    }
}
 