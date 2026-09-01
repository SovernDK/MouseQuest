using System;
using System.Collections.Generic;
using Atlas.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class Enemy
    {
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT = "Split/Left";
        protected const string SPLIT_RIGHT = "Split/Right";
        protected const string SPLIT_LEFT_GENERAL = "Split/Left/General";
        protected const string SPLIT_LEFT_GENERAL_SPLIT_RIGHT = "Split/Left/General/Split/Right";
        protected const string SPLIT_LEFT_GENERAL_SPLIT_RIGHT_NAME = "Split/Left/General/Split/Right/Name";

        [HorizontalGroup(SPLIT)]
        [BoxGroup(SPLIT_LEFT_GENERAL)]
        [HorizontalGroup(SPLIT_LEFT_GENERAL + "/Split", 55, LabelWidth = 90)]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Sprite icon;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        [ReadOnly] [HideInInspector]
        public int id;

        [HorizontalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT_NAME)] 
        public string name;

        [HorizontalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT_NAME)] 
        public bool transformed;

        [HorizontalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT_NAME)] 
        public int reward;

        [HorizontalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT_NAME)] 
        [ReadOnly]
        public float points;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        public GameObject attackEffect;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        public EElement attackElement;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        [InlineProperty]
        public Formula formula;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        public string battleBackScene;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        public Vector2 battlerScale;

        [VerticalGroup(SPLIT_LEFT)]
        [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        [OnInspectorGUI("CountPoints")]
        public List<AttributeValue> startingAttributeValues;

        [VerticalGroup(SPLIT_LEFT)]
        [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        [OnInspectorGUI("CountPoints")]
        public List<ResistanceValue> startingResistanceValues;

        [VerticalGroup(SPLIT_RIGHT)]
        public List<DialogueSequence> sequences;

        [VerticalGroup(SPLIT_RIGHT)]
        public List<Loot> loot;

        [VerticalGroup(SPLIT_RIGHT)]
        public List<Move> moves;

        public void Create()
        {
            //Attributes
            startingAttributeValues = new List<AttributeValue>();
            Array attributes = Enum.GetValues(typeof(EAttribute));

            for(int i = 0; i < attributes.Length; i++)
            {
                startingAttributeValues.Add(new AttributeValue() { attribute = (EAttribute) i });
            }

            //Resistances
            startingResistanceValues = new List<ResistanceValue>();
            Array resistances = Enum.GetValues(typeof(EElement));

            for(int i = 0; i < resistances.Length; i++)
            {
                startingResistanceValues.Add(new ResistanceValue() { element = (EElement) i });
            }
        }

        public void CountPoints()
        {
            float sum = 0;
            for(int i = 0; i < startingAttributeValues.Count; i++)
            {
                sum += startingAttributeValues[i].value;
            }

            for(int i = 0; i < startingResistanceValues.Count; i++)
            {
                sum += startingResistanceValues[i].value;
            }

            points = sum;
        }
    }

    [Serializable]
    public class DialogueSequence
    {
        public string conversation;
        [MinValue(1)]
        public int turn;
    }

    [Serializable]
    public class Move
    {
        [HorizontalGroup("Split")]
        [HorizontalGroup("Split/Left/Enabled")]
        [HideLabel, LabelWidth(0)]
        public bool enabled;

        [HorizontalGroup("Split/Left")]
        [HideLabel]
        [DisableIf("@enabled == false")]
        public EEnemyMove moveId;

        [HorizontalGroup("Split/Right", LabelWidth = 50)]
        [ProgressBar(0, 10)]
        [DisableIf("@enabled == false")]
        public int priority;

        [HorizontalGroup("Split/Right", LabelWidth = 100)]
        [MaxValue(5)]
        [DisableIf("@enabled == false")]
        public int weightGain;

        // [BoxGroup("Spell")]
        [LabelWidth(70)]
        [ShowIf("@moveId == EEnemyMove.Spellcast")]
        [DisableIf("@enabled == false")]
        public SpellPrototype spell;

        [FoldoutGroup("Transformation")]
        [ShowIf("@moveId == EEnemyMove.Transformation")]
        [DisableIf("@enabled == false")]
        public List<EnemyPrototype> enemies;

        [HorizontalGroup("Transformation/Split")]
        [ShowIf("@moveId == EEnemyMove.Transformation")]
        [DisableIf("@enabled == false")]
        public bool clearStatuses;

        [HorizontalGroup("Transformation/Split")]
        [ShowIf("@moveId == EEnemyMove.Transformation")]
        [DisableIf("@enabled == false")]
        public bool transferHp;

        [FoldoutGroup("Conditions")]
        [InlineProperty]
        [DisableIf("@enabled == false")]
        public TurnConditions turnCondition;
    
        [FoldoutGroup("Conditions")]
        [TableList(AlwaysExpanded = true)]
        [DisableIf("@enabled == false")]
        public List<AttributeConditions> attributeConditions;

        [FoldoutGroup("Conditions")]
        [TableList(AlwaysExpanded = true)]
        [DisableIf("@enabled == false")]
        public List<StateConditions> stateConditions;
    }

    [Serializable]
    public class AttributeConditions
    {
        public EAttribute attributeId;
        public ECondition condition;
        public int value;
        public EValueType valueType;
    }

    [Serializable]
    public class TurnConditions
    {
        [HorizontalGroup("TurnCondition")]
        public bool enable;

        [HorizontalGroup("TurnCondition")]
        [EnableIf("@enable == true")]
        public bool after;
        
        [HorizontalGroup("TurnCondition")]
        [EnableIf("@enable == true")]
        public int turn;
    }

    [Serializable]
    public class StateConditions
    {
        public BattlerStatePrototype state;
        public ETarget target;
        public bool exist;
    }

    public enum EEnemyMove
    {
        Attack, Spellcast, Transformation
    }

    public enum ECondition
    {
        Equal, LessThen, MoreThen, LessOrEqual, MoreOrEqual
    }

    public enum EValueType
    {
        Percent, Absolute
    }
}
