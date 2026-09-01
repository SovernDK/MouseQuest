using UnityEngine;
using Sirenix.OdinInspector;
using DamageNumbersPro;
using Sirenix.Utilities;
using ParadoxNotion.Design;
using System.Collections.Generic;
using Atlas.DB;

namespace Atlas.Utility 
{
    [GlobalConfig("Assets/Resources/Configs/")]
    public class Config : GlobalConfig<Config>
    {
        private const string LEFT_HORIZONTAL = "LeftHorizontal";
        private const string CENTER_HORIZONTAL = "LeftHorizontal/CenterHorizontal";
        private const string RIGHT_HORIZONTAL = "LeftHorizontal/CenterHorizontal/RightHorizontal";

        private const string ATTRIBUTES_BOX = "Attributes";
        private const string BATTLE_BOX = "Battle";
        private const string RIGHT_BOX = "RIGHT_BOX";

        [HorizontalGroup(LEFT_HORIZONTAL)] [BoxGroup(LEFT_HORIZONTAL + "/" + ATTRIBUTES_BOX)]
        public Vector2Int attributeValueLimits;

        [BoxGroup(LEFT_HORIZONTAL + "/" + ATTRIBUTES_BOX)]
        public Vector2Int hpValuesLimit;

        [BoxGroup(LEFT_HORIZONTAL + "/" + ATTRIBUTES_BOX)]
        [PreviewField]
        public Sprite castNormal;

        [BoxGroup(LEFT_HORIZONTAL + "/" + ATTRIBUTES_BOX)]
        [PreviewField]
        public Sprite castQuick;

        #region Battle
        [HorizontalGroup(CENTER_HORIZONTAL)] 
        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public GameObject defaultAttackEffect;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public GameObject defaultSpellEffect;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public GameObject defaultFailEffect;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public DamageNumber damageNumbers;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        [Range(1,99)]
        public float baseAttackHitChance;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public string riskyAttackDebuffFormula;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public int riskyAttackDebuffTime;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX)]
        public float battleStatesInterval;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX + "/AI")]
        public float weightReset = 5f;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX + "/AI")]
        public float weightIncrease = 1f;

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX + "/AI")]
        public Vector2Int priorityRandomness = new Vector2Int(-1, 1);

        [BoxGroup(CENTER_HORIZONTAL + "/" + BATTLE_BOX + "/Progression")]
        public List<EnemyPrototype> progression;
        #endregion

        #region UI      

        [TabGroup("UI")]
        [SerializeField]
        private Color _attributePositive;

        [TabGroup("UI")]
        [SerializeField] 
        private Color _attributeNegative;

        [TabGroup("UI")]
        [SerializeField] 
        private Color _attributeNormal;
        #endregion

        #region Sfx 
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _getHitSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _slashSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _clawSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _deathSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _battleTriggerSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _battlerFadeOut;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _addBattleLootSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _addBattleExpSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _battleWonSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _battleLostSfx;

        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _stepSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _buttonHoverSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _buttonClickSfx;
        
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _teleportSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _goldSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _lootPunchSfx;
        [SerializeField] [TabGroup("Sfx")]
        private AudioClip _levelUp;
        #endregion

        public AudioClip GetHitSfx { get => _getHitSfx; set => _getHitSfx = value; }
        public AudioClip SlashSfx { get => _slashSfx; set => _slashSfx = value; }
        public AudioClip StepSfx { get => _stepSfx; set => _stepSfx = value; }
        public AudioClip ButtonHoverSfx { get => _buttonHoverSfx; set => _buttonHoverSfx = value; }
        public AudioClip ButtonClickSfx { get => _buttonClickSfx; set => _buttonClickSfx = value; }
        public AudioClip TeleportSfx { get => _teleportSfx; set => _teleportSfx = value; }
        public AudioClip BattleTriggerSfx { get => _battleTriggerSfx; set => _battleTriggerSfx = value; }
        public AudioClip BattlerFadeOut { get => _battlerFadeOut; set => _battlerFadeOut = value; }
        public AudioClip BattleWonSfx { get => _battleWonSfx; set => _battleWonSfx = value; }
        public AudioClip BattleLostSfx { get => _battleLostSfx; set => _battleLostSfx = value; }
        public AudioClip AddBattleLootSfx { get => _addBattleLootSfx; set => _addBattleLootSfx = value; }
        public AudioClip AddBattleExpSfx { get => _addBattleExpSfx; set => _addBattleExpSfx = value; }
        public Color AttributePositive { get => _attributePositive; set => _attributePositive = value; }
        public Color AttributeNegative { get => _attributeNegative; set => _attributeNegative = value; }
        public Color AttributeNormal { get => _attributeNormal; set => _attributeNormal = value; }
        public AudioClip GoldSfx { get => _goldSfx; set => _goldSfx = value; }
        public AudioClip LootPunchSfx { get => _lootPunchSfx; set => _lootPunchSfx = value; }
        public AudioClip LevelUp { get => _levelUp; set => _levelUp = value; }
        public AudioClip DeathSfx { get => _deathSfx; set => _deathSfx = value; }
    }
}