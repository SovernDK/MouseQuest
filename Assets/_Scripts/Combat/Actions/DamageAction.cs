using Atlas.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat
{
    public class DamageAction : Action
    {
        private ETarget _target;
        private Formula _formula;
        private EElement _element;
        private bool _userElement;

    public int FinalDamageValue { get; set; }
        public float FinalDamageRatio { get; set; }

        public DamageAction(ETarget target, Formula formula, EElement element, bool userElement = false)
        {
            _target = target;
            _formula = formula;
            _element = element;
            _userElement = userElement;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical = false)
        {
            EElement element = _userElement ? user.BaseAtkElement : _element;

            switch(_target)
            {
                case ETarget.User:
                    user.TakeDamage(CalculateDamage(user, user, critical), element);
                    break;
                case ETarget.Other:
                    target.TakeDamage(CalculateDamage(user, target, critical), element);
                    break;
                case ETarget.Both:
                    user.TakeDamage(CalculateDamage(user, user, critical), element);
                    target.TakeDamage(CalculateDamage(user, target, critical), element);
                    break;
            }            
            ActionLogEntry = new AttackCommandLog(target, FinalDamageValue);
            ConsoleProDebug.LogToFilter($"{user.name} deals {element} Damage: {FinalDamageValue} to {_target}", "BattleSystem");

            return base.ExecuteAction(user, target, critical);
        }

        private int CalculateDamage(Battler user, Battler target, bool critical)
        {
            EElement element = _userElement ? user.BaseAtkElement : _element;
            int value = (int) _formula.Parse(user, target);
            float resistanceValue = target.Attributes.GetResistance(element) / 100; 

            FinalDamageValue = value - Mathf.RoundToInt(value * resistanceValue);
            FinalDamageValue += Mathf.RoundToInt(Random.Range(0f, .1f) * FinalDamageValue);

            //Critical Damage
            FinalDamageValue = critical ? FinalDamageValue * 3 : FinalDamageValue;
            FinalDamageValue = Mathf.Clamp(FinalDamageValue, 1, int.MaxValue);
            FinalDamageRatio = Mathf.Clamp(FinalDamageValue / target.Attributes.GetMaxValue(EAttribute.Hitpoints), .1f, 1f);

            return FinalDamageValue;
        }
    }
}