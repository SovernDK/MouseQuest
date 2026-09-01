using System.Collections;
using UnityEngine;

namespace Atlas.Player
{
    public class OverworldPlayerState : PlayerState
    {
        public OverworldPlayerState(PlayerFSM _context, int _stateId) : base(_context, _stateId)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Context.FSMContext.Controller.CanMove = true;
            // Context.FSMContext.Controller.enabled = true;
        }

        public override IEnumerator OnUpdate()
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                Context.FSMContext.InventorySystem.Toogle();
                Context.FSMContext.AttributeSystem.Toogle();
            }

            if(Input.GetKeyUp(KeyCode.Q))
            {
                Context.FSMContext.SpellSystem.Toggle();
            }

            if(Context.FSMContext.TriggerInRange != null && (Input.GetKeyUp(KeyCode.F) || Context.FSMContext.TriggerInRange.TouchTrigger))
            {
                ConsoleProDebug.LogToFilter("Triggering", "Player");
                Context.FSMContext.TriggerInRange.Trigger();
            }

            if(Context.FSMContext.PlayerCommands.Count > 0)
            {
                yield return Context.FSMContext.PlayerCommands.Dequeue().Execute();
                Context.FSMContext.UpdateUI();
            }
            
            // yield return Context.FSMContext.Controller.OnUpdate();
            yield return null;
        }

        public override void OnExit()
        {
            base.OnExit();
            Context.FSMContext.Controller.CanMove = false;
            // Context.FSMContext.Controller.enabled = false;
        }
    }
}