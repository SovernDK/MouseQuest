using System.Collections;
using UnityEngine;

public class LootBattleState : BattleState
{
    public LootBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
        
    }

    public override void OnEnter()
    {

    }

    public override IEnumerator OnUpdate()
    {
        FSMContext.AddLoot();
        FSMContext.ShowEndButton(true);

        yield return FSMContext.AnimateBattleResult();

        if(FSMContext.IsLevelUp)
        {
            Context.SetState(EBattleState.LevelUp);
        }
        else 
        {   
            // FSMContext.FadeOutBattleResult();
            Context.SetState(EBattleState.End);
        }
    }

    public override void OnExit()
    {

    }
}