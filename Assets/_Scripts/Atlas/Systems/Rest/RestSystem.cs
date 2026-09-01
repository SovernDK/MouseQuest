using Atlas.Core;
using Atlas.Presenters;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Atlas.Systems
{
    public class RestSystem : MonoBehaviour
    {
        [Inject]
        private RestPresenter _presenter;

        private FSMOwner _fsm;

        public UnityEvent Entered; 
        public UnityEvent Exited; 
        public UnityEvent ToNextBattle; 

        private void Awake() 
        {
            Initialize();
        }

        public void Initialize()
        {
            _presenter.System = this;
            _fsm = GetComponent<FSMOwner>();
        }

        public void EnterCampsite()
        {
            // SetState(ERestState.Camp);
            // _fsm.enabled = true;
            // _presenter.Enter();
            // Entered.Invoke();
        }

        public void UpdateState()
        {
             
        }
        
        public void NextBattle()
        {
            // End();
            // ToNextBattle.Invoke();
            // FindAnyObjectByType<GameStateSystem>().SetState(EGameState.Battle);
        }

        public void End()
        {
            _presenter.End();
            _fsm.enabled = false;
            Exited.Invoke();
        }

        public void Exit()
        {
            // _presenter.End();
            // _fsm.enabled = false;
            // FindAnyObjectByType<GameStateSystem>().SetState(EGameState.GameOver);
        }

        public void SetState(ERestState state)
        {
            _fsm.SetExposedParameterValue("state", state);
        }

        public void SetState(int stateId)
        {
            _fsm.SetExposedParameterValue("state", (ERestState) stateId);
        }
    }

    public enum ERestState
    {
        Camp = 0,
        Battle = 1,
        End = 2
    }
}