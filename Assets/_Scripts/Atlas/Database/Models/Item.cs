using System;
using System.Collections.Generic;
using Atlas.Enums;
using Atlas.Utility;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class Item
    {
        protected const string LEFT_VERTICAL_GROUP             = "Split/Left";
        protected const string STATS_BOX_GROUP                 = "Split/Left/Stats";
        protected const string GENERAL_SETTINGS_VERTICAL_GROUP = "Split/Left/General Settings/Split/Right";

        [HideInInspector]
        public int id;

        #region General
        [VerticalGroup(LEFT_VERTICAL_GROUP)]
        [HorizontalGroup(LEFT_VERTICAL_GROUP + "/General Settings/Split", 100, LabelWidth = 67)]
        [HideLabel, PreviewField(100)]
        public Sprite icon;

        [BoxGroup(LEFT_VERTICAL_GROUP + "/General Settings")]
        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        [ValueDropdown("GetTerms")]
        public string name;

        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        [ValueDropdown("GetTerms")]
        public string description = "item_default_description";

        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        public EItemType itemType;

        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        public int cost;

        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        public GameObject effect;
        #endregion

        [HorizontalGroup("Split", 0.5f, MarginLeft = 5, LabelWidth = 130)]
        [BoxGroup("Split/Right", LabelText = "Notes")]
        [HideLabel, TextArea(4, 9)]
        public string Notes;

        #region Equipable

        [ToggleGroup("Split/Left/equipable", "Equipable")]
        public bool equipable;
        
        [ToggleGroup("Split/Left/equipable")]
        public EEquipmentSlot equipmentSlot;

        [ToggleGroup("Split/Left/equipable")]
        [ShowIf("@itemType == EItemType.Weapon")]
        public EElement element;

        [ToggleGroup("Split/Left/equipable")]
        [ShowIf("@itemType == EItemType.Weapon")]
        [InlineProperty]
        public Formula formula;

        [ToggleGroup("Split/Left/equipable")]
        [TableList(AlwaysExpanded = true)]
        public List<AttributeModifier> modifiers;
        #endregion

        #region Consumable
        [ToggleGroup("Split/Left/consumable", "Consumable")]
        public bool consumable;

        [ToggleGroup("Split/Left/consumable")]
        [TableList(AlwaysExpanded = true)]
        public List<ActionType> actions;

        #endregion

        #region Cooking

        [ToggleGroup("Split/Left/cooking", "Cooking")]
        public bool cooking;
        
        [ToggleGroup("Split/Left/cooking", "Cooking")]
        [ShowIf("@itemType == EItemType.Component")]
        [TableList(AlwaysExpanded = true)]
        public List<AttributeModifier> cookingBenefits;

        #endregion

        private List<string> GetTerms()
        {
            return LocalizationManager.GetTermsList();
        }

        public void Create(string newName)
        {
            name = Util.ToItemSnakeCase(newName, "item_");
            description = Util.ToItemSnakeCase(newName, "item_", "_description");
        }

        public bool IsEmpty()
        {
            return name == "item_empty";
        }
    }

    [Serializable]
    public class AttributeModifier
    {
        public EAttribute id;
        public EAttributeModifierSource source;
        public int value;
    }

    public enum EItemType
    {
        Weapon, Armor, Component, Other
    }
}