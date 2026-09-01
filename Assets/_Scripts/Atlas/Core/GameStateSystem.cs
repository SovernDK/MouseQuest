using System;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using UnityEngine;

namespace Atlas.Core
{
    public class GameStateSystem : MonoBehaviour
    {
        [SerializeField]
        private FSMOwner _fsm;

        public void SetState(EGameState eGameState)
        {
            //Sometimes it just doesn't execute !?
            _fsm.SetExposedParameterValue("state", eGameState);
        }

        public void SetState(int stateId)
        {
            Debug.Log($"Setting state: {stateId}");
            _fsm.SetExposedParameterValue("state", (EGameState) stateId);
        }

        public void SetLoadingState()
        {
            _fsm.SetExposedParameterValue("state", EGameState.Loading);
        }

        public void SetRestState()
        {
            _fsm.SetExposedParameterValue("state", EGameState.Campfire);
        }
        
        public void SetBattleState()
        {
            _fsm.SetExposedParameterValue("state", EGameState.Battle);
        }
    }

    public enum EGameState
    {
        MainMenu = 0, 
        Loading = 1, 
        Campfire = 2, 
        Battle = 3, 
        GameOver = 4,
        CharacterCreator = 5
    }
}