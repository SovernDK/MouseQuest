using System.Collections;
using UnityEngine;

public class TurnEndBattleState : BattleState
{ 
    public TurnEndBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {

    }

    public override void OnEnter()
    {

    }

    public override IEnumerator OnUpdate()
    {
        FSMContext.EndTurn = false;

        if(!FSMContext.Battlers[0].Alive) 
        {
            // Debug.Log("Player dead");
            Context.SetState(EBattleState.Lost);
        }
        else if(!FSMContext.Battlers[1].Alive)
        {
            // Debug.Log("Enemy dead");
            Context.SetState(EBattleState.Death);
        }
        else 
        {
            Context.SetState(EBattleState.TurnStart);
        }

        yield return null;
    }

    public override void OnExit()
    {

    }
}