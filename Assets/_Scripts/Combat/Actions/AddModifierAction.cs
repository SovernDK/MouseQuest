using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class AddModifierAction : Action
    {
        private ETarget _target;
        private Formula _formula;
        private int _attribute;
        private string _source;
        private bool _isTimeLimited;
        private int _turnCounter;

        public AddModifierAction(ETarget target, Formula formula, int attribute, string source, bool isTimeLimited, int turnCounter = -1)
        {
            _target = target;
            _formula = formula;            
            _attribute = attribute;
            _source = source;
            _isTimeLimited = isTimeLimited;
            _turnCounter = turnCounter;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical = false)
        {
            int value = (int) _formula.Parse(user, target);
            
            switch(_target)
            {
                case ETarget.User:
                    user.AddModifier((EAttribute) _attribute, _source, value, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Other:
                    target.AddModifier((EAttribute) _attribute, _source, value, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Both:
                    user.AddModifier((EAttribute) _attribute, _source, value, _isTimeLimited, _turnCounter);
                    target.AddModifier((EAttribute) _attribute, _source, value, _isTimeLimited, _turnCounter);
                    break;
            }            

            ConsoleProDebug.LogToFilter($"Target {ETarget.User} received modifier {(EAttribute) _attribute} / {value} / {_turnCounter}", $"BattleSystem");
            
            return base.ExecuteAction(user, target, critical);
        }
    }
}