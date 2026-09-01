using System.Collections;
using UnityEngine;
using DG.Tweening;
using Atlas.Player;

public class StartBattleState : BattleState
{
    public StartBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
    }

    public override void OnEnter()
    {
        foreach (Battler battler in Context.FSMContext.Battlers)
        {
            battler.OnBattleStart();
        }
    }

    public override IEnumerator OnUpdate()
    {
        yield return Context.FSMContext.TransitionsSystem.FadeIn();

        Context.FSMContext.PrepareUIForNewBattle();
        FSMContext.CameraSystem.SwitchCamera(ECamera.Battle);
        yield return FSMContext.FadeOutBattlers();
        yield return FSMContext.TransitionsSystem.FadeOut();
        yield return FSMContext.FadeInBattlers();

        Context.FSMContext.SetBattleLogVisibility(true);
        Context.SetState(EBattleState.TurnStart);
    }

    public override void OnExit()
    {
        
    }
}