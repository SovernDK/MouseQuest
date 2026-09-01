using TMPro;
using UnityEngine;

public class LogEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _logValue;

    public void SetEntry(BattleLogEntry entry)
    {
        _logValue.text = entry.GetLogValue();
    }
}
