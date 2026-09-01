using Atlas.DB;
using DG.Tweening;
using I2.Loc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookingComponentCell : MonoBehaviour
{
    [SerializeField]
    private Image _icon;   
    [SerializeField]
    private TMPro.TMP_Text _name;   
    [SerializeField]
    private Button _button;

    public UnityEvent<int> OnClicked { get; set; }
    public int CellId { get; set; }
    public int ItemId { get; set; }

    public void Initialize(int cellId)
    {
        CellId = cellId;

        OnClicked = new UnityEvent<int>();
        _button.onClick.AddListener(() => { OnClicked.Invoke(CellId); });
    }

    public void ApplyItem(Item item)
    {
        if(item.id == ItemId) return;

        ItemId = item.id;

        _icon.sprite = item.icon;

        if(item.id == 0)
            _icon.DOFade(0,0);
        else
            _icon.DOFade(1,0);
        // _name.text = LocalizationManager.GetTranslation(item.name);

        // _button.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength, _vibrato);
    }
}
