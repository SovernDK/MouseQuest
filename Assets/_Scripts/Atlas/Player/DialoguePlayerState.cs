using System.Collections;
using UnityEngine;

namespace Atlas.Player
{
    public class DialoguePlayerState : PlayerState
    {
        public DialoguePlayerState(PlayerFSM _context, int _stateId) : base(_context, _stateId)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("Dialogue Started");
            base.OnEnter();
        }

        public override IEnumerator OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Dialogue Next Line");
                Context.FSMContext.DialogueSystem.NextLine();
            }
            
            yield return null;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}