using Atlas.DB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookingBenefitCell : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _name;   
    [SerializeField]
    private TMPro.TMP_Text _value;   
    [SerializeField]
    private Button _button;

    public UnityEvent<int> OnClicked { get; set; }
    public int CellId { get; set; }
    public int ItemId { get; set; }

    public void Initialize(int cellId)
    {
        CellId = cellId;

        // OnClicked = new UnityEvent<int>();
        // _button.onClick.AddListener(() => { OnClicked.Invoke(CellId); });
    }

    public void ApplyBenefit(AttributeModifier benefit)
    {
        _name.text = benefit.id.ToString(); //LocalizationManager.GetTranslation(benefit.ToString());
        _value.text = (benefit.value > 0) ? $"+{benefit.value}" : $"-{benefit.value}";
    }
}
