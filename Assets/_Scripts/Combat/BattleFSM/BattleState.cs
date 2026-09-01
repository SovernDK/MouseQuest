using System.Collections;
using Atlas.Core;

public class BattleState : IState<BattleFSM>
{
    public BattleFSM Context { get; set; }
    public BattleSystem FSMContext { get; set; }
    public int StateId { get; set; }

    public BattleState(BattleFSM context, int stateId)
    {
        Context = context;
        StateId = stateId;
        FSMContext = Context.FSMContext;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual IEnumerator OnUpdate()
    {
        yield return null;
    }
}