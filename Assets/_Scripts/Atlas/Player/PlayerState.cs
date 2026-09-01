using System.Collections;
using Atlas.Core;

namespace Atlas.Player
{
    public class PlayerState : IState<PlayerFSM>
    {
        public PlayerFSM Context { get; set; }
        public int StateId { get; set; }

        public PlayerState(PlayerFSM _context, int _stateId)
        {
            Context = _context;
            StateId = _stateId;
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
}