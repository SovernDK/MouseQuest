using System.Collections;

public class ExecuteCommandsBattleState : BattleState
{
    public ExecuteCommandsBattleState(BattleFSM context, int stateId) : base(context, stateId)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ConsoleProDebug.LogToFilter("ExecuteCommands OnEnter", "BattleFSM");
        ConsoleProDebug.LogToFilter($"Commands Count {FSMContext.Commands.Count}", "BattleFSM");
    }

    public override IEnumerator OnUpdate()
    {
        while(FSMContext.Commands.Count > 0)
        {
            yield return FSMContext.NextCommand();
    
            if(!FSMContext.CurrentBattler.Alive || !FSMContext.OtherBattler.Alive)
            {
                ConsoleProDebug.LogToFilter("Someone Died", "BattleFSM");
                break;
            }
        }
        
        Context.SetState(EBattleState.TurnEnd);
        yield return null;
    }

    public override void OnExit()
    {
        base.OnExit();
        ConsoleProDebug.LogToFilter("ExecuteCommands OnExit", "BattleFSM");
    }
}