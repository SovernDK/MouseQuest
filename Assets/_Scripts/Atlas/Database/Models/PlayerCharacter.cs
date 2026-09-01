using System;
using System.Collections.Generic;
using Atlas.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class PlayerCharacter
    {
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT = "Split/Left";
        protected const string SPLIT_RIGHT = "Split/Right";
        protected const string SPLIT_LEFT_GENERAL = "Split/Left/General";

        [HorizontalGroup(SPLIT)]
        [BoxGroup(SPLIT_LEFT_GENERAL)]
        [HorizontalGroup(SPLIT_LEFT_GENERAL + "/IconSplit", 55, LabelWidth = 70)]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Sprite icon;

        [VerticalGroup(SPLIT_LEFT_GENERAL + "/IconSplit/Right")]
        public string name;

        [VerticalGroup(SPLIT_LEFT_GENERAL + "/IconSplit/Right")]
        public EElement baseElement;

        [VerticalGroup(SPLIT_LEFT_GENERAL + "/IconSplit/Right")]
        [InlineProperty]
        public Formula baseAtkFormula;

        [VerticalGroup(SPLIT_LEFT_GENERAL + "/IconSplit/Right")]
        [InlineProperty]
        public Formula riskAtkFormula;

        [VerticalGroup(SPLIT_LEFT)]
        [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        public List<AttributeValue> startingAttributeValues;

        [VerticalGroup(SPLIT_LEFT)]
        [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        public List<ResistanceValue> startingResistanceValues;

        [VerticalGroup(SPLIT_RIGHT)]
        public List<InventoryValue> startingInventory;
         
        [VerticalGroup(SPLIT_RIGHT)]
        [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        public List<EquipmentValue> startingEquipment;

        [VerticalGroup(SPLIT_RIGHT)]
        public List<SpellPrototype> startingSpellbook;

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

            //Equipment
            startingEquipment = new List<EquipmentValue>();
            Array equipmentSlots = Enum.GetValues(typeof(EEquipmentSlot));

            for(int i = 0; i < equipmentSlots.Length; i++)
            {
                startingEquipment.Add(new EquipmentValue() { slot = (EEquipmentSlot) i });
            }
        }
    }

    [Serializable]
    public class AttributeValue
    {
        [ReadOnly]
        public EAttribute attribute;
        public int value;
    }

    [Serializable]
    public class ResistanceValue
    {
        [ReadOnly]
        public EElement element;
        [Range(-100, 100)]
        public int value;
    }

    [Serializable]
    public class InventoryValue
    {
        [HorizontalGroup("Horiz")]
        [HideLabel]
        public ItemPrototype item;
        
        [HorizontalGroup("Horiz")]
        [Range(1, 99)]
        public int amount = 1;
    }

    [Serializable]
    public class EquipmentValue
    {
        public EEquipmentSlot slot;
        [ValidateInput("ValidateSlotType", "Slot types are incompatible!")]
        public ItemPrototype item;

        public bool ValidateSlotType()
        {
            return item == null || item.item.equipable && item.item.equipmentSlot == slot;
        }
    }
}
