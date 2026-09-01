using System.Collections;

namespace Atlas.Player
{
    public class BattlePlayerState : PlayerState
    {
        public BattlePlayerState(PlayerFSM _context, int _stateId) : base(_context, _stateId)
        {
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override IEnumerator OnUpdate()
        {
            return base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}