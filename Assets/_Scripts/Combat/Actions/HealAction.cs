using Atlas.Enums;

namespace Combat
{
    public class HealAction : Action
    {
        private ETarget _target;
        private Formula _formula;

        public int FinalHealValue { get; set; }

        public HealAction(ETarget target, Formula formula)
        {
            _target = target;
            _formula = formula;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical)
        {
            int value = (int) _formula.Parse(user, target);
            FinalHealValue = value;

            switch(_target)
            {
                case ETarget.User:
                    user.Heal(value);
                    break;
                case ETarget.Other:
                    target.Heal(value);
                    break;
                case ETarget.Both:
                    user.Heal(value);
                    target.Heal(value);
                    break;
            }            

            return base.ExecuteAction(user, target);
        }
    }
}