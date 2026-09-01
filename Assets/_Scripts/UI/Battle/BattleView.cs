using System.Collections;
using System.Collections.Generic;
using Atlas.Core;
using Atlas.UI;
using CharacterSheet;
using DB;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Atlas.Effects;
using Atlas.DB;
using Atlas.Battle;
using Combat;
using Atlas.Utility;
using Atlas.Player;
using Atlas.Enums;

public class BattleView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    
    [SerializeField]
    private Image _frame;

    [SerializeField] [FoldoutGroup("TMP")]
    private TMP_Text _enemyName;
    [SerializeField] [FoldoutGroup("TMP")]
    private TMP_Text _playerHP;
    [SerializeField] [FoldoutGroup("TMP")]
    private GameObject _hpSlider;

    [SerializeField] [FoldoutGroup("BattleCommands")]
    private List<GameObject> _battleCommands;

    [SerializeField] [FoldoutGroup("BattlerSprite")]
    private GameObject _leftBattlerParent;
    [SerializeField] [FoldoutGroup("BattlerSprite")]
    private GameObject _rightBattlerParent;
    [SerializeField] [FoldoutGroup("BattlerSprite")]
    private Transform _rightBattlerLabel;
    [SerializeField] [FoldoutGroup("BattlerSprite")]
    private GameObject _leftBattlerLabel;

    private Animator _leftbattler;
    private Animator _rightbattler;

    [SerializeField] [FoldoutGroup("BattleInventory")]
    private GameObject _inventoryCellPrefab;
    [SerializeField] [FoldoutGroup("BattleInventory")]
    private Transform _inventoryContent;
    [SerializeField] [FoldoutGroup("BattleInventory")]
    private Transform _inventoryPanel;

    [SerializeField] [FoldoutGroup("BattleSpell")]
    private Transform _spellsPanel;
    [SerializeField] [FoldoutGroup("BattleSpell")]
    private Transform _spellsContent;
    [SerializeField] [FoldoutGroup("BattleSpell")]
    private GameObject _spellCellPrefab;
    [FoldoutGroup("BattleSpell")]
    private List<SpellCell> _spellCells;

    [SerializeField] [FoldoutGroup("BattleLog")]
    private Transform _logWindow;
    [SerializeField] [FoldoutGroup("BattleLog")]
    private Transform _logContent;
    [SerializeField] [FoldoutGroup("BattleLog")]
    private GameObject _logEntryPrefab;

    [SerializeField] [FoldoutGroup("BattleResult")]
    private Transform _battleResultContent;
    [SerializeField] [FoldoutGroup("BattleResult")]
    private TMP_Text _battleResultText;

    [SerializeField] [FoldoutGroup("Loot")]
    private GameObject _loot;
    [SerializeField] [FoldoutGroup("Loot")]
    private Transform _lootContent;
    [SerializeField] [FoldoutGroup("Loot")]
    private GameObject _lootRowPrefab;
    [SerializeField] [FoldoutGroup("Loot")]
    private TMP_Text _goldLoot;

    [SerializeField] [FoldoutGroup("LevelUp")]
    private Transform _levelUpWindow;
    [SerializeField] [FoldoutGroup("LevelUp")]
    private Transform _levelUpContent;
    [SerializeField] [FoldoutGroup("LevelUp")]
    private Transform _levelUpAttributesContent;
    [SerializeField] [FoldoutGroup("LevelUp")]
    private GameObject _levelUpAttributeRow;

    [SerializeField] [FoldoutGroup("Exp")]
    private GameObject _expContent;
    [SerializeField] [FoldoutGroup("Exp")]
    private TMP_Text _expLabel;
    [SerializeField] [FoldoutGroup("Exp")]
    private TMP_Text _expIncreaseValue;
    [SerializeField] [FoldoutGroup("Exp")]
    private TMP_Text _expCurrent;
    [SerializeField] [FoldoutGroup("Exp")]
    private Slider _expSlider;

    [SerializeField] [FoldoutGroup("End")]
    private Transform _endButton;

    [SerializeField] [FoldoutGroup("Shake")]
    private float _duration;
    [SerializeField] [FoldoutGroup("Shake")]
    private float _strength;
    [SerializeField] [FoldoutGroup("Shake")]
    private int _vibrato;

    private List<InventoryCell> _inventoryCells;

    public string ViewName => "Battle";
    public bool Visible => gameObject.activeSelf;

    public Transform Content { get => _content; set => _content = value; }
    [Inject]
    public BattlePresenter Presenter { get; set; }

    [Inject]
    private EffectsSystem Effects { get; set; }
    [Inject]
    private ResourcesSystem Resources { get; set; }
    [Inject]
    private ViewAnimation Animation { get; }
    [Inject]
    private CameraSystem CameraSystem { get; set; }
    [Inject]
    private Config Config { get; set; }

    public BattleEffectsAnchor Anchors { get; set; }
    public Animator LeftBattlerAnimator { get => _leftbattler; set => _leftbattler = value; }
    public Animator RightBattlerAnimator { get => _rightbattler; set => _rightbattler = value; }

    //Lets add some animation metods for UI in Views, like FadeIn etc...
    //It may be better to create scripts and attach them to UI components...
    public void Initialize()
    {
        Presenter.View = this;
        Anchors = Presenter.System.Anchors;
        _inventoryCells = new List<InventoryCell>();
        _spellCells = new List<SpellCell>();
    }

    public void ApplyInventory(InventorySlot[] slots)
    {
        for(int i = 0; i <  slots.Length; i++)
        {
            if(i < _inventoryCells.Count)
            {
                int cellId = _inventoryCells[i].CellId;

                // _inventoryCells[i].ApplyItem(Database.GetItem(slots[i].ItemId), slots[i].Amount);
                _inventoryCells[i].Connect(() => { Presenter.System.ConsumeItem(cellId); } );
            }
            else
            {
                string itemId = slots[i].ItemId;
                int cellId = slots[i].Id;

                GameObject cellClone = Instantiate(_inventoryCellPrefab, _inventoryContent);
                _inventoryCells.Add(cellClone.GetComponent<InventoryCell>());
                _inventoryCells[i].Initialize(cellId);
                _inventoryCells[i].ApplyItem(Database.Instance.GetItem(itemId), slots[i].Amount);
                // _inventoryCells[i].OnClicked.AddListener(Presenter.System.ConsumeItem);
                _inventoryCells[i].Connect(() => { Presenter.System.ConsumeItem(cellId); } );
            }
        }
    }

    public void ApplySpells(SpellEntry[] entries)
    {
        for(int i = 0; i < entries.Length; i++)
        {
            // if(i < _spellCells.Count)
            // {
            //     _spellCells[i].ApplySpell(entries[i], Database.Instance.GetSpell(entries[i].Id));
            // }
            // else
            // {
            //     GameObject cellClone = Instantiate(_spellCellPrefab, _spellsContent);
            //     _spellCells.Add(cellClone.GetComponent<SpellCell>());
            //     _spellCells[i].Initialize(entries[i].Id);
            //     _spellCells[i].ApplySpell(entries[i], Database.Instance.GetSpell(entries[i].Id));
            //     _spellCells[i].OnClicked.AddListener(Presenter.CastSpell);
            // }
        }
    }

    public void SetCommandsVisibility(bool visible)
    {
        foreach(GameObject command in _battleCommands)
        {
            if(visible)
                command.GetComponent<IViewAnimationHandler>().FadeIn(0);
            else
                command.GetComponent<IViewAnimationHandler>().FadeOut(0);
        }
    }

    public IEnumerator ShowCommands()
    {
        foreach(GameObject command in _battleCommands)
        {
            // Color newColor = command.GetComponentInChildren<TMP_Text>().color;
            // newColor.a = 1;
            // yield return command.GetComponentInChildren<TMP_Text>().DOColor(newColor, .1f).WaitForCompletion();
            yield return command.GetComponent<IViewAnimationHandler>().FadeIn(.5f).AsyncWaitForCompletion();
            yield return command.GetComponent<IViewAnimationHandler>().Unfold(.5f).AsyncWaitForCompletion();
            yield return new WaitForSeconds(.15f);
        }
    }

    public IEnumerator SetBattleResult(string text)
    {
        _battleResultText.text = LocalizationManager.GetTranslation(text);
        SetBattleResultVisibility(true);
        _battleResultContent.GetComponent<UILabel>().FadeIn(.75f);

        yield return _battleResultContent.GetComponent<UILabel>().Unfold(.75f).WaitForCompletion();
    }

    public void SetLevelUpVisibility(bool visible)
    {
        _levelUpWindow.gameObject.SetActive(visible);
    }

    public void ClearBattleLog()
    {
        foreach(Transform child in _logContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetBattleLogVisibility(bool visible)
    {
        _logWindow.gameObject.SetActive(visible);
    }

    public void SetBattleResultVisibility(bool visible)
    {
        if(visible) _battleResultContent.GetComponent<UILabel>().FadeIn(0);
        else _battleResultContent.GetComponent<UILabel>().FadeOut(0);
        // _battleResultContent.gameObject.SetActive(visible);
    }

    public void SetEndButtonVisibility(bool visible)
    {
        if(visible) _endButton.GetComponent<ButtonAdditional>().FadeIn(0);
        else _endButton.GetComponent<ButtonAdditional>().FadeOut(0);
    } 

    public void EnableEndButton(bool enabled)
    {
        _endButton.GetComponent<Button>().interactable = enabled;
        _endButton.GetComponent<Button>().image.raycastTarget = enabled;
    } 

    public void SetEnemyLabelVisibility(bool visible)
    {
        _rightBattlerLabel.gameObject.SetActive(visible);
    }

    public IEnumerator EnableCommands(bool enabled)
    {
        foreach(GameObject command in _battleCommands)
        {
            command.GetComponent<Button>().interactable = enabled;
            if(enabled)
            {
                command.GetComponent<ButtonAdditional>().FadeIn(.3f);
                command.GetComponent<ButtonAdditional>().Unfold(.3f);
            }
            else
            {
                command.GetComponent<ButtonAdditional>().FadeOut(.3f);
                command.GetComponent<ButtonAdditional>().Fold(.3f);
            }
            yield return new WaitForSeconds(.15f);
        }
    }

    public IEnumerator PlayNormalAttack(DamageAction action, Battler user, Battler target)
    {
        // ParticleEffect attackParticle = new ParticleEffect(user.BasicAttackEffect, target.BattlerPosition)
        //                                 .SetDelay(0f)
        //                                 .SetFollow(target.EffectsAnchor)
        //                                 .Build() as ParticleEffect;

        // SoundEffect attackSfx = new SoundEffect(Config.SlashSfx.name, target.BattlerPosition)
        //     .SetDelay(0f)
        //     .SetFollow(target.EffectsAnchor)
        //     .Build() as SoundEffect;

        // Effects.AddEffect(attackParticle);
        // Effects.AddEffect(attackSfx);

        // yield return user.PlayAnimation(EBattlerAnimation.Attack);
        // CameraSystem.Shake(0f, 5 * action.FinalDamageRatio);
        // Config.damageNumbers.Spawn(target.EffectsAnchor.position, action.FinalDamageValue);
        // SetPlayerAttributes(Presenter.PlayerAttributes);
        // yield return target.PlayAnimation(EBattlerAnimation.GetHit);

        yield return new WaitForSeconds(.5f); 
    }

    public IEnumerator PlaySpell(Spell spell, Action action, Battler user, Battler target)
    {
        ConsoleProDebug.LogToFilter($"User {user.name} is on side {user.Side}", "BattleSystem");
        // ConsoleProDebug.LogToFilter($"Target {target.name} is on side {target.Side} {Anchors.Get(target.Side, spell.targetAnchor).name}", "BattleSystem");

        Quaternion userRotation = (user.Side == EBattleAnchorSide.Right) ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);

        float targetEffectTime = 0;
        float userEffectTime = 0;

        // if(spell.userEffect != null)
        // {
        //     ParticleEffect userParticle = new ParticleEffect(spell.userEffect.name, Anchors.Get(user.Side, spell.targetAnchor).position)
        //                                 .SetDelay(0f)
        //                                 .SetRotation(userRotation)
        //                                 .SetTime(spell.userEffect.GetComponent<IEffect>().EffectTime)
        //                                 .Build() as ParticleEffect;
        //     userEffectTime = userParticle.Time;
        //     Effects.AddEffect(userParticle);
        // }

        // if(spell.targetEffect != null)
        // {
        //     ParticleEffect targetParticle = new ParticleEffect(spell.targetEffect.name, Anchors.Get(target.Side, spell.userAnchor).position)
        //                                 .SetDelay(0f)
        //                                 .SetFollow(Anchors.Get(target.Side, spell.userAnchor))
        //                                 .SetTime(spell.targetEffect.GetComponent<IEffect>().EffectTime)
        //                                 .Build() as ParticleEffect;
                                        
        //     targetEffectTime = targetParticle.Time;
        //     Effects.AddEffect(targetParticle);
        // }
       
        SetPlayerAttributes(Presenter.PlayerAttributes);

        float waitFor = (userEffectTime > targetEffectTime) ? userEffectTime : targetEffectTime;

        yield return new WaitForSeconds(waitFor); 
    }

    public IEnumerator FadeInLevelUp(CharacterSheet.Attribute[] attributes)
    {
        foreach(Transform child in _levelUpAttributesContent)
        {
            Destroy(child.gameObject);
        }

        _levelUpWindow.gameObject.SetActive(true);
        _levelUpWindow.GetComponent<Image>().DOFade(1,1);

        foreach (CharacterSheet.Attribute attribute in attributes)
        {
            yield return new WaitForSeconds(.25f);
            
            GameObject rowClone = Instantiate(_levelUpAttributeRow, _levelUpAttributesContent);
            rowClone.GetComponent<LevelUpAttributeRow>().Initialize();
            rowClone.GetComponent<LevelUpAttributeRow>().Apply(attribute);
            rowClone.GetComponent<LevelUpAttributeRow>().FadeIn();

            _levelUpWindow.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);
        }
    }

    public IEnumerator FadeInBattlers()
    {
        foreach(Transform child in _leftBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.DOColor(Color.white, .1f);
            }
        }
        
        yield return new WaitForSeconds(.1f);

        foreach(Transform child in _rightBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.DOColor(Color.white, .1f);
            }
        }

        yield return new WaitForSeconds(.1f);
    }

    public IEnumerator FadeOutBattlers()
    {
        foreach(Transform child in _leftBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.DOColor(Color.black, .1f);
            }
        }

        foreach(Transform child in _rightBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.DOColor(Color.black, .1f);
            }
        }

        yield return new WaitForSeconds(.1f);
    }

    public void SetBattlersVisibility(bool visible)
    {
        foreach(Transform child in _rightBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.enabled = visible;
            }
        }

        foreach(Transform child in _leftBattlerParent.transform)
        {
            if(child.gameObject.TryGetComponent(out SpriteRenderer sprite))
            {
                sprite.enabled = visible;
            }
        }
    }

    public IEnumerator Won()
    {
        _leftBattlerLabel.GetComponent<UILabel>().Fold(1.5f);
        // LeftBattlerAnimator.SetTrigger("dead");

        yield return SetBattleResult("ui_victory");
    }

    public IEnumerator Lost()
    {
        // Effects.AddSoundEffect(Gamemaster.Instance.Config.DeathSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
        // yield return _rightBattlerSprite.DOFade(0, 1f).WaitForCompletion();
        // RightBattler.SetTrigger("dead");
        SetBattlersVisibility(false);

        // Effects.AddSoundEffect(Gamemaster.Instance.Config.BattleLostSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
        yield return SetBattleResult("ui_lose");
    }

    public IEnumerator Shake(float multiplier = 1)
    {
        _enemyName.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength * multiplier, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);
        _hpSlider.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength * multiplier, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);
        
        yield return _frame.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength * multiplier, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);
    }

    public bool IsInventoryVisible()
    {
        return _inventoryPanel.gameObject.activeSelf;
    }

    public bool IsSpellsVisible()
    {
        return _spellsPanel.gameObject.activeSelf;
    }

    public void SetLogEntry(BattleLogEntry entry)
    {
        LogEntry entryClone = Instantiate(_logEntryPrefab, _logContent).GetComponent<LogEntry>();
        entryClone.SetEntry(entry);
    }

    public void SetInventoryVisibility(bool visible)
    {
        _inventoryPanel.gameObject.SetActive(visible);
    }

    public void SetSpellsVisibility(bool visible)
    {
        if(visible) 
        {
            _spellsPanel.gameObject.SetActive(visible);
            _spellsPanel.GetComponent<UILabel>().Unfold(.1f);
        }
        else 
        {
            _spellsPanel.GetComponent<UILabel>().Fold(.1f).OnComplete(() => _spellsPanel.gameObject.SetActive(visible));
        }
    }

    public IEnumerator AnimateBattleResult(List<Atlas.DB.Loot> lootTable, LevelModel level)
    {
        foreach(Transform child in _lootContent)
        {
            Destroy(child.gameObject);
        }

        // _goldLoot.DOFade(0,0);
        
        // _loot.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _loot.SetActive(true);
        
        _leftBattlerLabel.GetComponent<UILabel>().Unfold(1);
        _leftBattlerLabel.GetComponent<UILabel>().SetText("Loot");
        // yield return _loot.GetComponent<Image>().DOFade(1, 1).WaitForCompletion();

        // _goldLoot.text = "+" + lootTable.gold;
        // _goldLoot.DOFade(1, .5f);
        // Effects.AddSoundEffect(Gamemaster.Instance.Config.GoldSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);

        for(int i = 0; i < lootTable.Count; i++)
        {
            yield return new WaitForSeconds(.5f);

            GameObject go = Instantiate(_lootRowPrefab, _lootContent);
            go.GetComponent<LootRow>().Initialize(i, Resources);
            go.GetComponent<LootRow>().ApplyLoot(lootTable[i].prototype.item, lootTable[i].amount);
            go.GetComponent<LootRow>().FadeIn();
            
            // Effects.AddSoundEffect(Gamemaster.Instance.Config.AddBattleLootSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
            _loot.GetComponent<RectTransform>().DOShakeAnchorPos(_duration, _strength, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);
        }

        // _expContent.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        // _expSlider.value = 0;
        // _expContent.gameObject.SetActive(true);

        // yield return _expContent.GetComponent<Image>().DOFade(1, .5f).WaitForCompletion();
        // _expIncreaseValue.text = "+" + lootTable.exp;
        
        // _expSlider.GetComponent<RectTransform>().DOShakeAnchorPos(.5f, _strength, _vibrato, 90, false, true, ShakeRandomnessMode.Harmonic);

        // _expCurrent.text = level.CurrentExp + " / " +  level.NextLevelExp;
        
        // float sliderValue = level.CurrentExp / level.NextLevelExp;
        // Effects.AddSoundEffect(Gamemaster.Instance.Config.AddBattleExpSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
        // yield return _expSlider.DOValue(sliderValue, 2f).WaitForCompletion();

        foreach(Transform child in _lootContent)
        {
            yield return new WaitForSeconds(.25f);
            // Effects.AddSoundEffect(Gamemaster.Instance.Config.LootPunchSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
            child.GetComponent<LootRow>().Punch();
        }

        // yield return new WaitForSeconds(.25f);
        
        // _goldLoot.GetComponent<RectTransform>().DOPunchPosition(new Vector3(0, .5f, 0), .25f, 1, 1);
        // Effects.AddSoundEffect(Gamemaster.Instance.Config.LootPunchSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);
        // yield return _goldLoot.GetComponent<RectTransform>().DOPunchScale(new Vector3(.5f, .5f, 0), .25f, 1, 1);

        // yield return new WaitForSeconds(.25f);
    
        // _expIncreaseValue.GetComponent<RectTransform>().DOPunchPosition(new Vector3(0, 1, 0), .25f, 1, 1);
        // _expIncreaseValue.GetComponent<RectTransform>().DOPunchScale(new Vector3(1, 1, 0), .25f, 1, 1);
        // Effects.AddSoundEffect(Gamemaster.Instance.Config.LootPunchSfx.name, _leftBattlerSprite.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);
    }

    public void SetBattleLootVisibility(bool visible)
    {
        _loot.SetActive(false);        
        _expContent.gameObject.SetActive(false);
    }

    public void SetHPVisibility(bool visible)
    {
        if(visible) _hpSlider.GetComponent<IViewAnimationHandler>().FadeIn(0);
        else _hpSlider.GetComponent<IViewAnimationHandler>().FadeOut(0);
    }

    public void FadeInHP()
    {
        _hpSlider.GetComponent<IViewAnimationHandler>().FadeOut(0);
        _hpSlider.GetComponent<IViewAnimationHandler>().FadeIn(.5f);
        _hpSlider.GetComponent<IViewAnimationHandler>().Unfold(.75f);
    }

    public void SetPlayerAttributes(AttributesModel attr)
    {
        float value = (float) attr.GetValue(EAttribute.Hitpoints) / attr.GetMaxValue(EAttribute.Hitpoints);
        _hpSlider.GetComponent<Slider>().DOValue(value, .5f);
        _hpSlider.GetComponentInChildren<TMP_Text>().text = $"MAX: <b>{attr.GetMaxValue(EAttribute.Hitpoints)}";
    }

    public void SetEnemyName(string name)
    {
        _leftBattlerLabel.GetComponent<IViewAnimationHandler>().Unfold(.75f);
        _enemyName.text = name;
    }

    public void Hide()
    {
        Content.gameObject.SetActive(false);
    }

    public void Show()
    {
        Content.gameObject.SetActive(true);
    }

    public void Draw()
    {
        
    }
}
