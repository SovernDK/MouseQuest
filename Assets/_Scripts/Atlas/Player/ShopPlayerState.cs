using System.Collections;
using Atlas.Core;
using UnityEngine;

namespace Atlas.Player
{
    public class ShopPlayerState : PlayerState
    {
        public ShopPlayerState(PlayerFSM _context, int _stateId) : base(_context, _stateId)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override IEnumerator OnUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                // Context.FSMContext.ShopSystem.Close();
            }

            yield return null;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}