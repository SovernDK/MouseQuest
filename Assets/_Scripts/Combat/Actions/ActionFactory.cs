using Atlas.DB;

namespace Combat
{
    public static class ActionFactory
    {
        public static Action Create(ActionType action)
        {
            switch(action.actionType)
            {
                case EActionType.Heal:
                    return new HealAction(action.target, action.formula);
                case EActionType.Damage:
                    return new DamageAction(action.target, action.formula, action.element, action.userElement);
                case EActionType.AttributeIncrease:
                    return new AttributeIncreaseAction(action.target, action.formula, action.attribute);
                case EActionType.AttributeDecrease:
                    return new AttributeDeacreaseAction(action.target, action.formula, action.attribute);
                case EActionType.AddModifier:
                    return new AddModifierAction(action.target, action.formula, (int) action.attribute, action.source, action.turnLimit, action.turnLimitCount);
                case EActionType.AddState:
                    return new AddStateAction(action.target, action.state.state.name);
                case EActionType.RemoveState:
                    return new RemoveStateAction(action.target, action.allStates, action.formula);
                case EActionType.ChangeResistance:
                    return new ChangeResistanceAction(action.target, action.formula, action.element, action.source, action.turnLimit, action.turnLimitCount);
                case EActionType.ChangeAttackElement:
                    return new ChangeAttackElementAction(action.target, action.element, action.turnLimit, action.turnLimitCount);
                default:
                    return null;
            }
        }
    }
}
