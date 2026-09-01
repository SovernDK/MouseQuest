using System.Collections;
using System.Collections.Generic;
using Atlas.Core;

public class BattleFSM : IFiniteStateMachineAdvanced<BattleState, BattleSystem, EBattleState>
{
    public Dictionary<EBattleState, BattleState> States { get; set; }
    public BattleState CurrentState { get; set; }
    public Queue<IEnumerator> Commands { get; set; }
    public BattleSystem FSMContext { get; set; }

    public void Initialize(BattleSystem context)
    {
        FSMContext = context;
        
        States = new Dictionary<EBattleState, BattleState>()
        {
            { EBattleState.Start, new StartBattleState(this, (int) EBattleState.Start) },
            { EBattleState.TurnStart, new TurnStartBattleState(this, (int) EBattleState.TurnStart) },
            { EBattleState.Player, new PlayerTurnBattleState(this, (int) EBattleState.Player) },
            { EBattleState.Enemy, new EnemyTurnBattleState(this, (int) EBattleState.Enemy) },
            { EBattleState.ExecuteCommands, new ExecuteCommandsBattleState(this, (int) EBattleState.ExecuteCommands) },
            { EBattleState.TurnEnd, new TurnEndBattleState(this, (int) EBattleState.TurnEnd) },
            { EBattleState.Death, new DeathBattleState(this, (int) EBattleState.Death) },
            { EBattleState.Lost, new LostBattleState(this, (int) EBattleState.Lost) },
            { EBattleState.Target, new TargetBattleState(this, (int) EBattleState.Target) },
            { EBattleState.Loot, new LootBattleState(this, (int) EBattleState.Loot) },
            { EBattleState.LevelUp, new LevelUpBattleState(this, (int) EBattleState.LevelUp) },
            { EBattleState.End, new EndBattleState(this, (int) EBattleState.End) },
        };

        CurrentState = States[0];
        ConsoleProDebug.LogToFilter("Battle FSM Initialized", "BattleFSM");
    }

    public void SetState(int id)
    {
        CurrentState.OnExit();
        CurrentState = States[(EBattleState) id];
        CurrentState.OnEnter();
    }

    public void SetState(EBattleState id)
    {
        CurrentState.OnExit();
        CurrentState = States[id];
        CurrentState.OnEnter();
    }

    public IEnumerator Update()
    {
        yield return CurrentState.OnUpdate();
    }
}

public enum EBattleState
{
    Start, TurnStart, Player, Enemy, ExecuteCommands, TurnEnd, Target, Death, Lost, Loot, LevelUp, End
}