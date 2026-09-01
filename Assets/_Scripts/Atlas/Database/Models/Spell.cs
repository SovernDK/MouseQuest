using UnityEngine;
using I2.Loc;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Atlas.Enums;
using Atlas.Utility;

namespace Atlas.DB
{
    [Serializable]
    public class Spell
    {
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT = "Split/Left";
        protected const string SPLIT_RIGHT = "Split/Right";
        protected const string SPLIT_LEFT_GENERAL = "Split/Left/General";
        protected const string SPLIT_LEFT_EFFECT = "Split/Left/Effects";
        protected const string SPLIT_LEFT_GENERAL_SPLIT_RIGHT = "Split/Left/General/Split/Right";

        [HorizontalGroup(SPLIT)]
        [BoxGroup(SPLIT_LEFT_GENERAL)]
        [HorizontalGroup(SPLIT_LEFT_GENERAL + "/Split", 55, LabelWidth = 70)]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Sprite icon;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        [ValueDropdown("GetTerms")]
        public string name;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        [ValueDropdown("GetTerms")]
        public string description;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        [Range(1, 99)]
        public float hitChance;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        public ECastType castType;
        
        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        public PlayerCharacterPrototype playerCharacter;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        public bool onlyEnemy;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)] 
        [HideInInspector]
        public EElement element;

        [VerticalGroup(SPLIT_LEFT)]
        [BoxGroup(SPLIT_LEFT_EFFECT)] 
        public ETarget target;

        [BoxGroup(SPLIT_LEFT_EFFECT)] 
        public int maxPrepared;
        
        [BoxGroup(SPLIT_LEFT_EFFECT)]
        [Required]
        public GameObject targetEffect;

        [BoxGroup(SPLIT_LEFT_EFFECT)]
        [Required]
        public AudioClip targetSfxEffect;

        [BoxGroup(SPLIT_LEFT_EFFECT)]
        public Vector3 effectPositionOffset;

        [BoxGroup(SPLIT_RIGHT + "/notes", LabelText = "Notes")]
        [HideLabel, TextArea(4, 9)]
        public string notes;

        [VerticalGroup(SPLIT_RIGHT)]
        [ListDrawerSettings(DefaultExpandedState = true)]
        public List<ActionType> actions;

        private List<string> GetTerms()
        {
            return LocalizationManager.GetTermsList();
        }

        public void Create(string newName)
        {
            name = Util.ToItemSnakeCase(newName, "spell_");
            description = Util.ToItemSnakeCase(newName, "spell_", "_desc");
            hitChance = 99;
            targetEffect = Config.Instance.defaultSpellEffect;
        }
    }

    public enum ECastType
    {
        Quick, Normal
    }
}
