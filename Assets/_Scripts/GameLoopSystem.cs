using Atlas.Core;
using UnityEngine;

public class GameLoopSystem : MonoBehaviour
{
    public void BattleEnd(bool won)
    {
        if(won)
        {

        }
        else
        {
            Gamemaster.Instance.GameOver();
        }
    }
}
