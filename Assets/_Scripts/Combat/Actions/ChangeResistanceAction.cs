using Atlas.Enums;

namespace Combat
{
    public class ChangeResistanceAction : Action
    {
        private ETarget _target;
        private Formula _formula;
        private EElement _element;
        private string _source;
        private bool _isTimeLimited;
        private int _turnCounter;

        public ChangeResistanceAction(ETarget target, Formula formula, EElement element, string source, bool isTimeLimited, int turnCounter = -1)
        {
            _target = target;
            _formula = formula;            
            _element = element;
            _source = source;
            _isTimeLimited = isTimeLimited;
            _turnCounter = turnCounter;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical)
        {
           int value = (int) _formula.Parse(user, target);

            switch(_target)
            {
                case ETarget.User:
                    user.AddModifier(_element, _source, value, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Other:
                    target.AddModifier(_element, _source, value, _isTimeLimited, _turnCounter);
                    break;
                case ETarget.Both:
                    user.AddModifier(_element, _source, value, _isTimeLimited, _turnCounter);
                    target.AddModifier(_element, _source, value, _isTimeLimited, _turnCounter);
                    break;
            }

            return base.ExecuteAction(user, target);
        }
    }
}