using Atlas.DB;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlerStateCell : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TMP_Text _turnsLeft;
    [SerializeField]
    private MMF_Player _activated;
    [SerializeField]
    private MMF_Player _deactivated;

    public void SetState(BattlerState state)
    {
        _icon.sprite = state.icon;
        _turnsLeft.text = $"{state.turnsToExpire}";
    }

    private void OnEnable()
    {
        _activated.PlayFeedbacks();
    }

    private void OnDisable()
    {
        _deactivated.PlayFeedbacks();
    }
}
