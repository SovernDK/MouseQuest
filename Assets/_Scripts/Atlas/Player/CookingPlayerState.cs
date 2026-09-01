using System.Collections;
using UnityEngine;

namespace Atlas.Player
{
    public class CookingPlayerState : PlayerState
    {
        public CookingPlayerState(PlayerFSM _context, int _stateId) : base(_context, _stateId)
        {
            
        }

        public override void OnEnter()
        {
            // Debug.Log("Cooking Started");
            base.OnEnter();
        }

        public override IEnumerator OnUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                // Context.FSMContext.CookingSystem.Close();
            }
            
            yield return null;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}