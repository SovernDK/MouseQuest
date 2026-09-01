using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class MagicDamageAction : Action
    {
        private ETarget _target;
        private int _value;
        private Formula _formula;
        private EElement _elementId;

        public int FinalDamageValue { get; set; }
        public float FinalDamageRatio { get; set; }

        public MagicDamageAction(ETarget target, int value, EElement elementId, Formula formula)
        {
            _target = target;
            _value = value;
            _elementId = elementId;
            _formula = formula;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical)
        {
            ConsoleProDebug.LogToFilter($"{user.name} executes MagicDamageAction on {target.name}", "BattleSystem");
            ConsoleProDebug.LogToFilter($"{target.name}", "BattleSystem");

            switch(_target)
            {
                case ETarget.User:
                    user.TakeDamage(CalculateDamage(user, user), _elementId);
                    break;
                case ETarget.Other:
                    target.TakeDamage(CalculateDamage(user, target), _elementId);
                    break;
                case ETarget.Both:
                    user.TakeDamage(CalculateDamage(user, user), _elementId);
                    target.TakeDamage(CalculateDamage(user, target), _elementId);
                    break;
            }            
            
            ActionLogEntry = new MagicAttackCommandLog(target, FinalDamageValue);
            ConsoleProDebug.LogToFilter(user.name + " deals MAGIC Damage: " + FinalDamageValue + " to " + _target.ToString(), "BattleSystem");
            
            return base.ExecuteAction(user, target);
        }

        private int CalculateDamage(Battler user, Battler target)
        {
            int baseValue = (int) _formula.Parse(user, target);
            float resistanceValue = target.Attributes.GetResistance(_elementId) / 100; 

            // FinalDamageValue = _value + user.Attributes.GetValue(EAttribute.MagicAttack);
            FinalDamageValue = baseValue - Mathf.RoundToInt(_value * resistanceValue);
            FinalDamageValue += Mathf.RoundToInt(Random.Range(0f, .1f) * FinalDamageValue);

            //Critical Dmg
            FinalDamageValue = (Random.Range(0f, 1f) < .05f) ? FinalDamageValue * 3 : FinalDamageValue;
            FinalDamageRatio = Mathf.Clamp(FinalDamageValue / target.Attributes.GetMaxValue(EAttribute.Hitpoints), .1f, 1f);

            return FinalDamageValue;
        }
    }
}