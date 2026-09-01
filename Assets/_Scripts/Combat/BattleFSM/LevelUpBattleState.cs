using System.Collections;
using UnityEngine;

public class LevelUpBattleState : BattleState
{ 
    public LevelUpBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {

    }

    public override void OnEnter()
    {
    }

    public override IEnumerator OnUpdate()
    {
        yield return FSMContext.FadeInLevelUp();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
        FSMContext.SetLevelUpVisibility(false);
        FSMContext.AnimateBattleResult();

        Context.SetState(EBattleState.End);
    }

    public override void OnExit()
    {

    }
}