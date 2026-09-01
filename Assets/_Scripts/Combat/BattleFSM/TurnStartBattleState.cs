using System.Collections;

public class TurnStartBattleState : BattleState
{ 
    public TurnStartBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {

    }

    public override void OnEnter()
    {
        Context.FSMContext.Refresh();
    }

    public override IEnumerator OnUpdate()
    {
        FSMContext.EndTurn = false;

        if(!FSMContext.Battlers[1].Alive) 
        {
            Context.SetState(EBattleState.Loot);
        }

        if(Context.FSMContext.IsPlayerFirst())
        {
            ConsoleProDebug.LogToFilter("PLAYER goes FIRST", "BattleFSM");
            Context.SetState(EBattleState.Player);
        }
        else
        {
            ConsoleProDebug.LogToFilter("ENEMY goes FIRST", "BattleFSM");
            Context.SetState(EBattleState.Enemy);
        }
        yield return null;
    }

    public override void OnExit()
    {

    }
}