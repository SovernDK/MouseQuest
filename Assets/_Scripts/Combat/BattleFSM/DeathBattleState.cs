using System.Collections;
using UnityEngine;

public class DeathBattleState : BattleState
{ 
    public DeathBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {

    }

    public override void OnEnter()
    {

    }

    public override IEnumerator OnUpdate()
    {
        FSMContext.SetCommandsVisibility(false);
        FSMContext.SetBattleLogVisibility(false);
        FSMContext.SetEnemyLabelVisibility(false);

        yield return FSMContext.Won();
        // FSMContext.SetBattleResult("Victory!");

        Context.SetState(EBattleState.Loot);
    }

    public override void OnExit()
    {

    }
}