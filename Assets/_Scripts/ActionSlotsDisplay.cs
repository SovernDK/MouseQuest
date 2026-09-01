using TMPro;
using UnityEngine;

public class ActionSlotsDisplay : MonoBehaviour
{
    public TMP_Text quickSlot;
    public TMP_Text normalSlot;

    public void UpdateNormalSlot(BattleCommand command)
    {
        normalSlot.text = $"{command.Name}";
    }

    public void UpdateQuickSlot(BattleCommand command)
    {
        if(command != null)
            quickSlot.text = $"{command.Name}";
        else 
            quickSlot.text = "None";
    }

    public void Clear()
    {
        quickSlot.text = $"None";
        normalSlot.text = $"None";
    }
}
