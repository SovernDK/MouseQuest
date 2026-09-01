using System.Collections.Generic;
using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class ChangeAttackElementAction : Action
    {
        private ETarget _target;
        private EElement _element;
        private bool _isTimeLimited;
        private int _turnCounter;

        public ChangeAttackElementAction(ETarget target, EElement element, bool isTimeLimited, int turnCounter = -1)
        {
            _target = target;
            _element = element;
            _isTimeLimited = isTimeLimited;
            _turnCounter = turnCounter;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical = false)
        {
            switch(_target)
            {
                case ETarget.User:
                    user.Attributes.AddElementModifier(_element, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Other:
                    target.Attributes.AddElementModifier(_element, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Both:
                    user.Attributes.AddElementModifier(_element, _isTimeLimited, _turnCounter);
                    target.Attributes.AddElementModifier(_element, _isTimeLimited, _turnCounter);
                    break;
            }
            
            return base.ExecuteAction(user, target, critical);
        }
    }
}