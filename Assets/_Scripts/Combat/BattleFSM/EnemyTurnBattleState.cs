using System.Collections;
using UnityEngine;

public class EnemyTurnBattleState : BattleState
{
    public EnemyTurnBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ConsoleProDebug.LogToFilter("EnemyTurn OnEnter", "BattleFSM");
        
        Context.FSMContext.CurrentBattler = Context.FSMContext.Battlers[1];
        Context.FSMContext.OtherBattler = Context.FSMContext.Battlers[0];

        Context.FSMContext.CurrentBattler.OnTurnStart();
    }

    public override IEnumerator OnUpdate()
    {
        // Context.FSMContext.Wait = true;

        yield return Context.FSMContext.CurrentBattler.MakeMove();
        // if(Context.FSMContext.Commands.Count > 0)
        //     yield return Context.FSMContext.Commands.Dequeue().Execute();

        // Context.FSMContext.Wait = false;
        if(FSMContext.EndTurn)
        {
            Context.SetState(EBattleState.ExecuteCommands);
        }
        else
        {
            FSMContext.EndTurn = true;
            Context.SetState(EBattleState.Player);
        }

        // if(!Context.FSMContext.OtherBattler.Alive)
        // {
        //     Context.SetState(EBattleState.TurnEnd);
        // }
        // else if(FSMContext.EndTurn)
        // {
        //     Context.SetState(EBattleState.TurnEnd);
        // }
        // else if(!FSMContext.CurrentBattler.Busy && !FSMContext.EndTurn)
        // {
        //     FSMContext.EndTurn = true;
        //     Context.SetState(EBattleState.Player);
        // }
    }

    public override void OnExit()
    {
        base.OnExit();
        ConsoleProDebug.LogToFilter("EnemyTurn OnExit", "BattleFSM");
        Context.FSMContext.CurrentBattler.OnTurnEnd();
    }
}