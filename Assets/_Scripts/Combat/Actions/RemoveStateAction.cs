using System.Collections.Generic;
using Atlas.Enums;
using UnityEngine;

namespace Combat
{
    public class RemoveStateAction : Action
    {
        private ETarget _target;
        private bool _allStates;
        private Formula _formula;

        public RemoveStateAction(ETarget target, bool allStates, Formula formula)
        {
            _target = target;
            _allStates = allStates;
            _formula = formula;
        }

        public override Action ExecuteAction(Battler user, Battler target, bool critical = false)
        {
            int number = Mathf.RoundToInt(_formula.Parse(user, target));

            switch(_target)
            {
                case ETarget.User:
                    RemoveStates(user, number);
                    break;
                case ETarget.Other:
                    RemoveStates(target, number);
                    break;
                case ETarget.Both:
                    RemoveStates(user, number);
                    RemoveStates(target, number);
                    break;
            }
            
            return base.ExecuteAction(user, target, critical);
        }

        public void RemoveStates(Battler target, int number)
        {
            if(_allStates)
            {

            }
            else
            {
                List<CharacterSheet.State> toRemove = target.Attributes.States.GetRange(0, number);
                toRemove.ForEach(state => 
                {
                    target.RemoveState(state.Id);
                });
            }
        } 
    }
}