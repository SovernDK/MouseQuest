using Atlas.Enums;

namespace Combat
{
    public class AddStateAction : Action
    {
        private ETarget _target;
        private string _state;

        public AddStateAction(ETarget target, string state)
        {
            _target = target;
            _state = state;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical = false)
        {
            switch(_target)
            {
                case ETarget.User:
                    user.AddState(_state);
                    break;
                case ETarget.Other:
                    target.AddState(_state);
                    break;
                case ETarget.Both:
                    user.AddState(_state);
                    target.AddState(_state);
                    break;
            }
            
            return base.ExecuteAction(user, target, critical);
        }
    }
}