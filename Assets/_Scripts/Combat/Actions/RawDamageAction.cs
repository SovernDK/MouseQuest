using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class RawDamageAction : Action
    {
        private ETarget _target;
        private int _value;
        private EElement _elementId;

        public int FinalDamageValue { get; set; }
        public float FinalDamageRatio { get; set; }

        public RawDamageAction(ETarget target, int value, EElement elementId)
        {
            _target = target;
            _value = value;
            _elementId = elementId;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical)
        {
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
            
            return base.ExecuteAction(user, target);
        }

        private int CalculateDamage(Battler user, Battler target)
        {
            FinalDamageValue = _value;
            
            int maxHP = target.Attributes.GetMaxValue(EAttribute.Hitpoints);
            int ratio = FinalDamageValue / Mathf.Clamp(maxHP, 1, int.MaxValue);
            FinalDamageRatio = Mathf.Clamp(ratio, .1f, 1f);

            return FinalDamageValue;
        }
    }
}