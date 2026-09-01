using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommands : MonoBehaviour
{
    public List<GameObject> battleCommands;
    public List<GameObject> restCommands;

    public void ShowBattleCommands()
    {
        restCommands.ForEach(b => b.SetActive(false));    
        battleCommands.ForEach(b => b.SetActive(true));    
    }

    public void ShowRestCommands()
    {
        battleCommands.ForEach(b => b.SetActive(false));
        restCommands.ForEach(b => b.SetActive(true));    
    }
}
