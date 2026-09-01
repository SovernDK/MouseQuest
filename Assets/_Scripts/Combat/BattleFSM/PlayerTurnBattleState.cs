using System.Collections;
using UnityEngine;

public class PlayerTurnBattleState : BattleState
{
    public PlayerTurnBattleState(BattleFSM context, int stateId) : base(context, stateId) { }

    public override void OnEnter()
    {
        FSMContext.CurrentBattler = FSMContext.Battlers[0];
        FSMContext.OtherBattler = FSMContext.Battlers[1];
        
        FSMContext.CurrentBattler.OnTurnStart();
        FSMContext.Wait = false;

        ConsoleProDebug.LogToFilter("PlayerTurn OnEnter", "BattleFSM");
    }

    public override IEnumerator OnUpdate()
    {
        yield return FSMContext.EnableCommands(true);
        yield return new WaitUntil(() => !FSMContext.CurrentBattler.Busy);
        yield return FSMContext.EnableCommands(false);

        if(FSMContext.EndTurn)
        {
            Context.SetState(EBattleState.ExecuteCommands);
        }
        else
        {
            FSMContext.EndTurn = true;
            Context.SetState(EBattleState.Enemy);
        }

        // if(!FSMContext.OtherBattler.Alive)
        // {
        //     Context.SetState(EBattleState.TurnEnd);
        // }
        // else if(!FSMContext.CurrentBattler.Busy && FSMContext.EndTurn)
        // {
        //     Context.SetState(EBattleState.ExecuteCommands);
        // }
        // else if(!FSMContext.CurrentBattler.Busy && !FSMContext.EndTurn)
        // {   
        //     FSMContext.EndTurn = true;
        //     Context.SetState(EBattleState.Enemy);
        // }
    }

    public override void OnExit()
    {
        FSMContext.SetInventoryVisibility(false);
        FSMContext.SetSpellsVisibility(false);
        FSMContext.Refresh();

        FSMContext.CurrentBattler.OnTurnEnd();
        ConsoleProDebug.LogToFilter("PlayerTurn OnExit", "BattleFSM");
    }
}