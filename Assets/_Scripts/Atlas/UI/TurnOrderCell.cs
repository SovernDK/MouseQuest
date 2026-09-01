using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class TurnOrderCell : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;
    [SerializeField]
    private MMF_Player _activated;
    [SerializeField]
    private MMF_Player _deactivated;

    public void SetCombatantNameAndOrder(int turn, string name)
    {
        _label.text = $"{turn}.{name}";
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
