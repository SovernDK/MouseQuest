using DB;
using I2.Loc;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelUpAttributeRow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _oldValue;
    [SerializeField]
    private Image _arrow;
    [SerializeField]
    private TMP_Text _newValue;


    public void Initialize()
    {

    }

    public void Apply(CharacterSheet.Attribute attribute)
    {
        // _name.text = LocalizationManager.GetTranslation(_depot.Attributes[(int) attribute.Id].name);
        // _oldValue.text = attribute.BaseValue.ToString();
        if(attribute.VariableAttribute)
            _newValue.text = attribute.BaseMaxValue.ToString();
        else
            _newValue.text = attribute.BaseValue.ToString();
    }

    public void FadeIn()
    {
        _name.DOFade(1, .6f);
        // _oldValue.DOFade(1, .75f);
        _arrow.DOFade(1, .75f);
        _newValue.DOFade(1, .75f);
    }
}
