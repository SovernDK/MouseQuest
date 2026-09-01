using System.Collections;
using UnityEngine;

public class EndBattleState : BattleState
{
    public EndBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override IEnumerator OnUpdate()
    {
        Context.FSMContext.Wait = true;
        yield return new WaitForSeconds(1f);
        Context.FSMContext.Wait = false;
        
        Context.FSMContext.EndBattle();
    }

    public override void OnExit()
    {
        
    }
}