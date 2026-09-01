using System;
using Atlas.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class ActionType
    {
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT = "Split/Left";
        protected const string SPLIT_RIGHT = "Split/Right";
        protected const string SPLIT_RIGHT_TURNS = "Split/Right/Turns";

        [HideLabel]
        public EActionType actionType;

        [HorizontalGroup(SPLIT)]
        [VerticalGroup(SPLIT_LEFT)]
        public ETarget target;

        [VerticalGroup(SPLIT_LEFT)]
        [ShowIf("@actionType == EActionType.Damage || actionType == EActionType.ChangeResistance || actionType == EActionType.ChangeAttackElement")]
        [DisableIf("@userElement == true")]
        public EElement element;

        [VerticalGroup(SPLIT_LEFT)]
        [ShowIf("@actionType == EActionType.Damage")]
        public bool userElement;
        
        [VerticalGroup(SPLIT_RIGHT)]
        // [InfoBox("use a or b for battlers with b being the target and a user, you can access attribute values (a.hitpoints) and max values (a.hitpointsMax)")]
        [ShowIf("@actionType != EActionType.AddState")]
        [InlineProperty]
        public Formula formula;
 
        [VerticalGroup(SPLIT_RIGHT)]
        [ShowIf("@actionType == EActionType.AttributeIncrease || actionType == EActionType.AttributeDecrease || actionType == EActionType.AddModifier")]
        public EAttribute attribute;

        #region Add/Remove State
        [ShowIf("@actionType == EActionType.AddState")]
        [VerticalGroup(SPLIT_RIGHT)]
        public BattlerStatePrototype state;

        [ShowIf("@actionType == EActionType.RemoveState")]
        [InfoBox("If false remove amount of states from formula starting from first state")]
        [VerticalGroup(SPLIT_RIGHT)]
        public bool allStates;
        #endregion

        [VerticalGroup(SPLIT_RIGHT)]
        [ShowIf("@actionType == EActionType.AddModifier")]
        [HideInInspector]
        public string source;

        [VerticalGroup(SPLIT_RIGHT)]
        [MinMaxSlider(1, 12)]
        public Vector2Int repeat = new Vector2Int(1,1);

        [HorizontalGroup(SPLIT_RIGHT_TURNS)]
        [LabelWidth(75)]
        [ShowIf("@actionType == EActionType.AddModifier || actionType == EActionType.ChangeResistance || actionType == EActionType.ChangeAttackElement")]
        public bool turnLimit;

        [HorizontalGroup(SPLIT_RIGHT_TURNS, Width = 225)]
        [DisableIf("@turnLimit == false")]
        [LabelText("Count")]
        [ShowIf("@actionType == EActionType.AddModifier || actionType == EActionType.ChangeResistance || actionType == EActionType.ChangeAttackElement")]
        public int turnLimitCount;
    }

    public enum EActionType
    {
        Heal, Damage, AttributeIncrease, AttributeDecrease, AddModifier, AddState, RemoveState, ChangeResistance, ChangeAttackElement
    }
}