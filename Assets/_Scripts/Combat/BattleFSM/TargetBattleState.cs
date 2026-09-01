using System.Collections;
using UnityEngine;

public class TargetBattleState : BattleState
{
    public TargetBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
    }

    public override void OnEnter()
    {
    }

    public override IEnumerator OnUpdate()
    {
        yield return null;
    }

    public override void OnExit()
    {

    }
}