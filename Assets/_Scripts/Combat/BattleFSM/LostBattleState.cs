using System.Collections;
using UnityEngine;

public class LostBattleState : BattleState
{ 
    public LostBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {

    }

    public override void OnEnter()
    {

    }

    public override IEnumerator OnUpdate()
    {
        // FSMContext.SetCommandsVisibility(false);
        FSMContext.SetBattleLogVisibility(false);
        // FSMContext.SetEnemyLabelVisibility(false);

        yield return FSMContext.Lost();
        // FSMContext.SetBattleResult("Victory!");

        Context.SetState(EBattleState.End);
    }

    public override void OnExit()
    {

    }
}