using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class AttributeDeacreaseAction : Action
    {
        private ETarget _target;
        private Formula _formula;
        private EAttribute _attributeId;

        public AttributeDeacreaseAction(ETarget target, Formula formula, EAttribute attributeId)
        {
            _target = target;
            _formula = formula;
            _attributeId = attributeId;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical)
        {
            int value = Mathf.RoundToInt(_formula.Parse(user, target));
            
            switch(_target)
            {
                case ETarget.User:
                    user.Attributes.DecreaseAttribute(_attributeId, value);
                    break;
                case ETarget.Other:
                    target.Attributes.DecreaseAttribute(_attributeId, value);
                    break;
                case ETarget.Both:
                    user.Attributes.DecreaseAttribute(_attributeId, value);
                    target.Attributes.DecreaseAttribute(_attributeId, value);
                    break;
            }            

            return base.ExecuteAction(user, target);;
        }
    }
}