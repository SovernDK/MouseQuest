using System;
using System.Collections;
using System.Collections.Generic;
using Atlas.Battle;
using Atlas.Core;
using Atlas.DB;
using Atlas.Effects;
using Atlas.Enums;
using Atlas.Systems;
using Atlas.Utility;
using CharacterSheet;
using Combat;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Action = Combat.Action;
using DamageNumbersPro;

[Serializable]
public class Battler : MonoBehaviour 
{
    [SerializeField] [TitleGroup("General")]
    protected SpriteRenderer _battlerSprite;
    [SerializeField] [TitleGroup("General")]
    protected Transform _battlerSpritePivot;
    [SerializeField] [BoxGroup("Debug")]
    protected bool _invincible;

    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _enter;
    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _getHit;
    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _acting;
    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _heal;
    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _buffed;
    [SerializeField] [FoldoutGroup("Feel")]
    protected MMF_Player _debuffed;

    [SerializeField]
    protected DamageNumber _dmgNumbersType;
    [SerializeField]
    protected DamageNumber _healNumbersType;

    protected string _name;
    protected BattleCommand _normalCommand;
    protected BattleCommand _quickCommand;

    private string _basicAttackEffect;
    private string _basicAttackSfx;
    private Formula _baseAtkFormula = new Formula { expression = "a.attack * 2 - b.defence" };
    private Formula _riskyAtkFormula = new Formula { expression = "a.attack * 3 - b.defence" };
    private EElement _baseAtkElement = EElement.None;
    private bool _busy;
    private bool _alive;
    private bool _quickActionUsed;
    private int _turnCounter;

    [InjectOptional(Optional = true)]
    protected AtlasBattleSystem _battleSystem;

    [InjectOptional(Optional = true)]
    protected ResourcesSystem _resources;

    [Inject]
    protected EffectsSystem _effects { get; }

    public virtual AttributesModel Attributes { get; set; }
    public virtual SpellModel Spellbook { get; set; }
    public virtual EBattleAnchorSide Side { get; set; }
    public bool Busy { get => _busy; set => _busy = value; }
    public bool Alive { get => _alive; set => _alive = value; }
    public SpriteRenderer BattlerSprite { get => _battlerSprite; set => _battlerSprite = value; }
    public Transform BattlerTransform { get => _battlerSprite.transform; }
    public Vector3 BattlerPosition { get => _battlerSprite.transform.position; }
    public virtual string Name => _name;
    public virtual string BasicAttackEffect { get => _basicAttackEffect; set => _basicAttackEffect = value; }
    public virtual string BasicAttackSfx { get => Config.Instance.SlashSfx.name; set => _basicAttackSfx = value; }
    public virtual Formula BaseAtkFormula { get => _baseAtkFormula; set => _baseAtkFormula = value; }
    public virtual Formula RiskyAtkFormula { get => _riskyAtkFormula; set => _riskyAtkFormula = value; }
    public virtual EElement BaseAtkElement { get => Attributes.BaseElement; }
    public bool QuickActionUsed { get => _quickActionUsed; set => _quickActionUsed = value; }
    public BattleCommand NormalCommand { get => _normalCommand; set => _normalCommand = value; }
    public BattleCommand QuickCommand { get => _quickCommand; set => _quickCommand = value; }
    public int TurnCounter { get => _turnCounter; set => _turnCounter = value; }

    [FoldoutGroup("Events")]
    public UnityEvent<int, int> OnHPValueChanged;

    [FoldoutGroup("Events")]
    public UnityEvent<List<State>> OnBattlerStateChanged;
    [FoldoutGroup("Events")]
    public UnityEvent<CharacterSheet.Attribute[]> OnBattlerAddedModifier;

    [FoldoutGroup("Events")]
    public UnityEvent<bool> OnQuickActionUsedUp;

    [FoldoutGroup("Events")]
    public UnityEvent<string> OnSpellCast;

    [FoldoutGroup("Events")]
    public UnityEvent<BattleCommand> OnNormalCommandChosen;

    [FoldoutGroup("Events")]
    public UnityEvent<BattleCommand> OnQuickCommandChosen;

    public virtual void Initialize(AttributesModel attributes)
    {
        Attributes = attributes;
        
        Initialize();
    }

    public virtual void Initialize()
    {
        
    }

    public void SetCharacter(PlayerCharacter character)
    {
        BaseAtkFormula = character.baseAtkFormula;
    }

    public virtual void SetSprite(Sprite sprite)
    {
        _battlerSprite.sprite = sprite;
    }

    public virtual IEnumerator MakeMove()
    {
        yield return null;
    }

    #region Actions
    public virtual void UseQuickCommand()
    {
        _quickActionUsed = true;
        OnQuickActionUsedUp.Invoke(_quickActionUsed);
    }

    public virtual void CastSpell(string spellId)
    {
    }

    public virtual void AddNormalCommand(BattleCommand command)
    {
        _normalCommand = command;
        OnNormalCommandChosen.Invoke(command);
    }

    public virtual void AddQuickCommand(BattleCommand command)
    {
        _quickCommand = command;
        OnQuickCommandChosen.Invoke(command);
        UseQuickCommand();
    }

    public virtual void ClearCommands()
    {
        _normalCommand = null;
        _quickCommand = null;
    }
    #endregion

    public virtual void OnBattleStart()
    {
        _alive = true;
        _enter.PlayFeedbacks();
    }

    public virtual void OnTurnStart()
    {
        ClearCommands();

        _busy = true;

        _quickActionUsed = false;
        OnQuickActionUsedUp.Invoke(_quickActionUsed);
    }

    public virtual IEnumerator OnExecutionStarted()
    {
        foreach(State state in Attributes.States)
        {
            BattlerState bState = Database.Instance.GetBattlerState(state.Id);
            if(bState.activation == EBattlerStateActivation.OnExecution)
            {
                foreach(ActionType action in bState.actions) 
                { 
                    action.source = "state_" + state.Id;
                    Action a = ActionFactory.Create(action).ExecuteAction(this, this); 
                    yield return PlayAnimation(action.actionType, new AnimationData() { action = a });
                }
            }
        }

        Attributes.CountModifiersDown();
        Attributes.CountStatesDown();

        OnBattlerStateChanged.Invoke(Attributes.States);
    }

    public virtual void OnTurnEnd()
    {
        List<string> statesToRemove = new List<string>();
        Attributes.States.ForEach(state =>
            {
                BattlerState bState = Database.Instance.GetBattlerState(state.Id);
                if(bState.activation == EBattlerStateActivation.TurnEnd)
                {
                    foreach(ActionType action in bState.actions) 
                    { 
                        Action a = ActionFactory.Create(action).ExecuteAction(this, this); 
                        StartCoroutine(PlayAnimation(action.actionType, new AnimationData() { action = a }));
                    }
                    if(bState.frequency == EBattlerStateFrequency.Once) statesToRemove.Add(bState.name);
                }
            }
        );

        foreach(string state in statesToRemove)
        {
            Attributes.States.Remove(Attributes.States.Find(s => s.Id.Equals(state)));
        }

        OnBattlerStateChanged.Invoke(Attributes.States);
    }

    public virtual DamageAction GetAttackAction()
    {
        return null;
    }

    public virtual DamageAction GetRiskyAttackAction()
    {
        return null;
    }

    public virtual int TakeDamage(int value, EElement elementId)
    {
        if(_invincible) return 0;
        
        //Execute states which are activated on DamageTaken
        Attributes.DecreaseAttributeValue(EAttribute.Hitpoints, value);
        OnHPValueChanged.Invoke(Attributes.GetValue(EAttribute.Hitpoints), Attributes.GetMaxValue(EAttribute.Hitpoints));

        if(Attributes.GetValue(EAttribute.Hitpoints) <= 0)
        {
            Kill();
        }

        return value;
    }

    public virtual void Kill()
    {
        _alive = false;
        StartCoroutine(PlayDeath());
    }

    public virtual void Heal(int value)
    {
        Attributes.IncreaseAttributeValue(EAttribute.Hitpoints, value);
        OnHPValueChanged.Invoke(Attributes.GetValue(EAttribute.Hitpoints), Attributes.GetMaxValue(EAttribute.Hitpoints));
    }

    public virtual void AddState(string state)
    {
        Attributes.AddState(state);
        OnBattlerStateChanged.Invoke(Attributes.States);
    }

    public virtual void RemoveState(string state)
    {
        Attributes.RemoveState(state);
        OnBattlerStateChanged.Invoke(Attributes.States);
    }

    public virtual void ClearStates()
    {
        Attributes.RemoveAllStates();
        OnBattlerStateChanged.Invoke(Attributes.States);
    }

    public virtual void AddModifier(EAttribute type, string key, int value, bool isTimeLimited = false, int turnCount = -1)
    {
        Attributes.AddModifier(type, key, value, isTimeLimited, turnCount);
        OnBattlerAddedModifier.Invoke(Attributes.Attributes);
    }

    public virtual void AddModifier(EElement type, string key, int value, bool isTimeLimited = false, int turnCount = -1)
    {
        Attributes.AddModifier(type, key, value, isTimeLimited, turnCount);
        OnBattlerAddedModifier.Invoke(Attributes.Attributes);
    }

    public IEnumerator PlayAnimation(EActionType type, AnimationData data = new AnimationData())
    {
        switch(type)
        {
            case EActionType.Damage:
                yield return PlayGetHit(data.duration, (data.action as DamageAction).FinalDamageValue); break;
            case EActionType.Heal:
                yield return PlayHealed((data.action as HealAction).FinalHealValue); break;
            case EActionType.AddState:
                yield return PlayBuff(); break;
            case EActionType.AddModifier:
                yield return PlayBuff(); break;
            case EActionType.ChangeResistance:
                yield return PlayBuff(); break;
            case EActionType.ChangeAttackElement:
                yield return PlayBuff(); break;
            default: 
                yield return PlayGetHit(data.duration, data.damageValue);
                break;
        }
    }

    public void ChangeAttackElement(EElement element)
    {
        _baseAtkElement = element;
    }

    protected virtual IEnumerator PlayGetHit(float duration, float value)
    {
        _dmgNumbersType.Spawn(BattlerPosition, value);
        _getHit.PlayFeedbacks();
        yield return new WaitForSeconds(_getHit.TotalDuration);
    }

    public virtual IEnumerator PlayActing()
    {
        Vector3 endValue = Vector3.one * 1.25f;
        yield return _battlerSpritePivot.DOScale(endValue, .3f).WaitForCompletion();
    }

    public virtual IEnumerator PlayEndActing()
    {
        Vector3 endValue = Vector3.one;
        yield return _battlerSpritePivot.DOScale(endValue, .3f);
    }

    protected virtual IEnumerator PlayGetHitRepeated(int count)
    {
        yield return null;
    }

    public virtual IEnumerator PlayHealed(float value)
    {
        _healNumbersType.Spawn(BattlerPosition, value);
        _heal.PlayFeedbacks();
        yield return new WaitForSeconds(_heal.TotalDuration);
    }

    public virtual IEnumerator PlayBuff()
    {
        SoundEffect spellSfx = new SoundEffect("BuffCombo_0", BattlerTransform.position)
            .Build() as SoundEffect;
        _effects.AddEffect(spellSfx);

        ParticleEffect spellEffect = new ParticleEffect("BaseBuffEffect", BattlerTransform.position)
            .SetDelay(0f)
            .Build() as ParticleEffect;
        _effects.AddEffect(spellEffect);

        _buffed.PlayFeedbacks();
        yield return new WaitForSeconds(_buffed.TotalDuration);
    }

    public virtual IEnumerator PlayDebuff()
    {
        SoundEffect spellSfx = new SoundEffect("Spell_Fizzle_1_S", BattlerTransform.position)
            .Build() as SoundEffect;
        _effects.AddEffect(spellSfx);

        ParticleEffect spellEffect = new ParticleEffect("BaseDebuffEffect", BattlerTransform.position)
            .SetDelay(0f)
            .Build() as ParticleEffect;
        _effects.AddEffect(spellEffect);

        _debuffed.PlayFeedbacks();
        yield return new WaitForSeconds(_debuffed.TotalDuration);
    }

    public virtual IEnumerator PlayAttack()
    {
        yield return null;
    }

    public virtual IEnumerator PlayDeath()
    {
        Hide();

        yield return null;
    }

    public virtual void Hide()
    {
        _battlerSprite.enabled = false;
    }

    public virtual void Show()
    {
        _battlerSprite.enabled = true;
    }

    public float GetPropertyValue(string propertyKey)
    {
        Dictionary<string, float> properties = new Dictionary<string, float>();
        
        foreach(EAttribute attribute in Enum.GetValues(typeof(EAttribute)))
        {
            string key = Enum.GetName(typeof(EAttribute), attribute);
            float value = Attributes.GetValue(attribute);

            properties.Add(key.ToLower(), value);
        }

        foreach(EAttribute attribute in Enum.GetValues(typeof(EAttribute)))
        {
            string key = Enum.GetName(typeof(EAttribute), attribute);
            float value = Attributes.GetMaxValue(attribute);

            properties.Add(key.ToLower() + "Max", value);
        }
        
        return properties[propertyKey];
    }

}

public struct AnimationData
{
    public Action action;
    public float duration;
    public float delay;
    public float damageValue;
    public float healValue;
}

public enum EBattlerAnimation
{
    GetHit = 0, 
    Cast = 1,
    Heal = 2, 
    Buff = 3, 
    Debuff = 4,
    Attack = 5
}