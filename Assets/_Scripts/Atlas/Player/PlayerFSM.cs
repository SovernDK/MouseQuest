using System.Collections;
using System.Collections.Generic;
using Atlas.Core;

namespace Atlas.Player
{
    public class PlayerFSM : IFiniteStateMachine<PlayerState, PlayerSystem, EPlayerState>
    {
        public Dictionary<EPlayerState, PlayerState> States { get; set; }
        public PlayerState CurrentState { get; set; }
        public PlayerSystem FSMContext { get; set; }

        public void Initialize(PlayerSystem context)
        {
            FSMContext = context;
            
            States = new Dictionary<EPlayerState, PlayerState>()
            {
                { EPlayerState.Overworld, new OverworldPlayerState(this, (int) EPlayerState.Overworld) },
                { EPlayerState.Battle, new BattlePlayerState(this, (int) EPlayerState.Battle) },
                { EPlayerState.Dialogue, new DialoguePlayerState(this, (int) EPlayerState.Dialogue) },
                { EPlayerState.Shop, new ShopPlayerState(this, (int) EPlayerState.Shop) },
                { EPlayerState.Cooking, new CookingPlayerState(this, (int) EPlayerState.Cooking) },
            };

            CurrentState = States[0];
            CurrentState.OnEnter();
            ConsoleProDebug.LogToFilter("SetState: Overworld", "PlayerFSM");
        }

        public void SetState(int id)
        {
            ConsoleProDebug.LogToFilter($"SetState: {id}", "PlayerFSM");
            CurrentState.OnExit();
            CurrentState = States[(EPlayerState) id];
            CurrentState.OnEnter();
        }

        public void SetState(EPlayerState id)
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

    public enum EPlayerState 
    {
        Overworld, Battle, Dialogue, Shop, Cooking
    }
}