using System.Collections;
using System.Collections.Generic;
using Atlas.Battle;
using Atlas.Core;
using Atlas.DB;
using Atlas.Effects;
using Atlas.Pooling;
using Combat;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using UnityEngine.Rendering;
using Atlas.Player;
using Atlas.Enums;

public class BattleSystem : MonoBehaviour, ISystem
{
    [SerializeField]
    private PlayerBattler _playerBattler;
    [SerializeField]
    private EnemyBattler _enemyBattler;
    [SerializeField]
    private BattleEffectsAnchor _anchors;
    [SerializeField]
    private Transform _enemyBattlersRoot;

    private BattleFSM _battleFSM;
    private Battle _currentBattle;
    private Enemy _currentEnemy;
    private bool _isRunning;
    
    private List<EnemyBattler> _enemies;
    private Battler[] _battlers;
    private Battler _currentBattler;
    private Battler _otherBattler;
    private BattleLog _battleLog;

    private Queue<BattleCommand> _commands;
    private Queue<BattleViewEffectCommand> _effectCommands;

    private Coroutine _battleLoop;

    private string _currentVolumeProfile;

    [Inject]
    public PoolSystem Pool { get; }
    [Inject]
    public BattlePresenter Presenter { get; }
    [Inject]
    public ResourcesSystem Resources { get; }
    [Inject]
    public PlayerSystem Player { get; }
    [Inject]
    public ActionSystem ActionSystem { get; }
    [InjectOptional(Optional = true)]
    public TransitionsSystem TransitionsSystem { get; }
    [Inject]
    public EffectsSystem EffectsSystem { get; }
    [Inject]
    public CameraSystem CameraSystem { get; }
    [Inject]
    public Volume _currentVolume { get; }

    public UnityEvent BattleEnd { get; set; }
    public Battler[] Battlers { get => _battlers; set => _battlers = value; }
    public Battler CurrentBattler { get => _currentBattler; set => _currentBattler = value; }
    public Queue<BattleCommand> Commands { get => _commands; }
    public Queue<BattleViewEffectCommand> EffectCommands { get => _effectCommands; }
    public Battler OtherBattler { get => _otherBattler; set => _otherBattler = value; }
    public BattleLog BattleLog { get => _battleLog; set => _battleLog = value; }
    public bool Wait { get; set; }
    public Battle CurrentBattle { get => _currentBattle; set => _currentBattle = value; }
    public Enemy CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }
    public bool EndTurn { get; set; }
    public bool IsLevelUp { get; set; }
    public BattleEffectsAnchor Anchors { get => _anchors; set => _anchors = value; }
    public EnemyBattler EnemyBattler { get => _enemyBattler; set => _enemyBattler = value; }
    public PlayerBattler PlayerBattler { get => _playerBattler; set => _playerBattler = value; }

    private void Awake() 
    {
        Initialize();
    }

    public void Initialize()
    {
        Presenter.System = this;
        
        BattleEnd = new UnityEvent();

        _battleFSM = new BattleFSM();
        _battleFSM.Initialize(this);

        _battleLog = new BattleLog();
        _battleLog.NewEntryAdded.AddListener(UpdateLogEntry);

        // _enemies = new List<EnemyBattler>();
        // foreach(Transform child in _enemyBattlersRoot)
        // {
        //     _enemies.Add(child.GetComponent<EnemyBattler>());
        // }

        _commands = new Queue<BattleCommand>();
        _effectCommands = new Queue<BattleViewEffectCommand>();
        _battlers = new Battler[2];

        _battlers[0] = _playerBattler;
        (_battlers[0] as PlayerBattler).Player = Player;
        _battlers[0].Initialize();

        _battlers[1] = _enemyBattler;
        _battlers[1].Initialize();
    }

    public void StartBattle(int battleId)
    {
        ConsoleProDebug.LogToFilter($"{_battlers[0]}", "BattleSystem");
        _currentVolumeProfile = _currentVolume.profile.name;
        _currentVolume.profile = Resources.LoadVolumeProfile("Battle");
        ConsoleProDebug.LogToFilter($"Loaded profile {_currentVolume.profile.name}", "BattleSystem");

        Player.PlayerFSM.SetState(EPlayerState.Battle);

        _currentBattle = Database.Instance.GetBattle(battleId);
        _currentEnemy = _currentBattle.enemyPrototype.data;
    
        // _enemies.ForEach(enemy => enemy.gameObject.SetActive(false));

        // _enemyBattler = _enemies.Find(enemy => enemy.name.Equals(_currentEnemy.name));
        _battlers[1] = _enemyBattler.CreateBattlerFromEnemy(_currentEnemy);
        _battlers[1].Initialize();
        _enemyBattler.gameObject.SetActive(true);

        _battleLoop = StartCoroutine(Loop());
    }

    public void PrepareUIForNewBattle()
    {
        IsLevelUp = false;
        // ClearBattleLog();

        Presenter.StartBattle(CurrentBattle, PlayerBattler, CurrentEnemy);
        Refresh();
    }

    public void Refresh()
    {
        // Presenter.ApplyInventory(Player.InventorySystem.GetItemsByType(EItemType.Component));
        Presenter.ApplyInventory(Player.InventorySystem.GetConsumables());
        Presenter.ApplyAttributes(Player.AttributeSystem.Attributes);
        Presenter.ApplySpells(Player.SpellSystem.SpellModel.Spells);
    }

    public void EndBattle()
    {
        Pool.ReturnAllByPool("Sfx");

        StopCoroutine(_battleLoop);
        Player.PlayerFSM.SetState(EPlayerState.Overworld);

        _isRunning = false;

        Presenter.EndBattle();
        BattleEnd.Invoke();

        _currentVolume.profile = Resources.LoadVolumeProfile(_currentVolumeProfile);
        CameraSystem.SwitchCamera(ECamera.World);
    }

    public void FleeBattle()
    {
        _battleFSM.SetState(EBattleState.End);
        EndBattle();
    }

    public IEnumerator SetBattleResult(string text)
    {
        yield return Presenter.SetBattleResult(text);
    }

    public void SetEnemyLabelVisibility(bool visible)
    {
        Presenter.SetEnemyLabelVisibility(visible);
    }

    public IEnumerator FadeInCommands()
    {
        yield return Presenter.FadeInCommands();
    }

    public void SetBattleLogVisibility(bool visible)
    {
        Presenter.SetBattleLogVisibility(visible);
    }

    public IEnumerator EnableCommands(bool enabled)
    {
        yield return Presenter.EnableCommands(enabled);
    }

    public IEnumerator FadeInBattlers()
    {
        Presenter.FadeInHP();
        yield return Presenter.FadeInBattlers();
    }

    public IEnumerator FadeOutBattlers()
    {
        yield return Presenter.FadeOutBattlers();
    }

    public void ToggleInventory()
    {
        SetInventoryVisibility(!Presenter.IsInventoryVisible());
    }

    public void ToggleSpells()
    {
        SetSpellsVisibility(!Presenter.IsSpellsVisible());
    }

    public void SetInventoryVisibility(bool visible)
    {
        Presenter.SetInventoryVisibility(visible);
    }

    public void SetSpellsVisibility(bool visible)
    {
        Presenter.SetSpellsVisibility(visible);
    }

    public bool IsPlayerFirst()
    {
        float speedPlayer = _battlers[0].Attributes.GetValue(EAttribute.Speed);
        float speedEnemy = _battlers[1].Attributes.GetValue(EAttribute.Speed);

        float totalSpeed = speedEnemy + speedPlayer;
        float playerProbability = speedPlayer / totalSpeed;
        
        // If you want to change the weighting 
        // (e.g., make the player have a greater chance of going first even when the speed difference is large), 
        // you could modify how the probability is calculated, for example by squaring the speed values:
        // float playerProbability = Mathf.Pow(playerSpeed, 2) / (Mathf.Pow(playerSpeed, 2) + Mathf.Pow(enemySpeed, 2));

        float randomValue = Random.Range(0f, 1f);
        // Debug.Log("Random: " + randomValue);

        return randomValue < playerProbability;
    }
    
    public IEnumerator PlayNormalAttack(DamageAction action, Battler user, Battler target)
    {
        yield return Presenter.PlayNormalAttack(action, user, target);
    } 

    public IEnumerator PlaySpell(Spell spell, Action action, Battler user, Battler target)
    {
        yield return Presenter.PlaySpell(spell, action, user, target);
    } 

    #region Commands
    private void AddCommand(BattleCommand newCommand)
    {
        CurrentBattler.Busy = false;
        _commands.Enqueue(newCommand);
    }

    private void AddEffectCommand(BattleViewEffectCommand newCommand)
    {
        _effectCommands.Enqueue(newCommand);
    }

    public void AddAttackCommand()
    {
        //Here and in BattleCommand you're doing same thing
        //AddEffectCommand(new AttackEffectCommand(this, _currentBattler, _otherBattler));
        // AddCommand(new AttackBattleCommand(this, CurrentBattler, OtherBattler));
    }

    public void AddSpellCommand(string spellId)
    {
        // if(CurrentBattler.Spellbook != null && CurrentBattler.Spellbook.Spells[spellId].Amount > 0)
        // {
        //     CurrentBattler.Spellbook.DecreaseSpellAmount(spellId, 1);

        //     Presenter.ApplySpells(CurrentBattler.Spellbook.Spells);
        //     Presenter.SetSpellsVisibility(false);
        // }
    }

    public IEnumerator NextCommand()
    {
        if(_commands.Count > 0)
        {
            // yield return EffectCommands.Dequeue().Execute();
            yield return Commands.Dequeue().Execute();
        }
    }
    #endregion

    public void NewLogEntry(BattleLogEntry logEntry)
    {
        _battleLog.NewLogEntry(logEntry);
    }

    public void UpdateLogEntry(BattleLogEntry logEntry)
    {
        Presenter.UpdateBattleLog(logEntry);
    }

    public void ClearBattleLog()
    {
        Presenter.ClearBattleLog();
    }

    public IEnumerator AnimateBattleResult()
    {
        // DB.Models.LootTable _currentLootTable = Depot.LootTables.Find(table => table.battleId == _currentBattle.id);
        // if(_currentLootTable == null) Debug.LogError("ERROR! Enemy with id " + _currentEnemy.id + " (" + _currentEnemy.name + ") has NO loot table!!!");
        yield return Presenter.AnimateBattleResult(_currentEnemy.loot, Player.AttributeSystem.Level);
    }

    public void SetBattleLootVisibility(bool visible)
    {
        Presenter.SetBattleLootVisibility(visible);
    }

    public void SetBattleResultVisibility(bool visible)
    {
        Presenter.SetBattleResultVisibility(visible);
    }

    public void SetCommandsVisibility(bool visible)
    {
        Presenter.SetCommandsVisibility(visible);
    }

    public IEnumerator FadeOutBattleResult()
    {
        yield return null;
    }

    public void AddLoot()
    {
        // LootTable _currentLootTable = Depot.LootTables.Find(table => table.battleId == _currentBattle.id);
        // foreach(DB.Models.Loot loot in _currentLootTable.loot)
        // {
        //     Player.InventorySystem.AddItem(loot.itemId, loot.amount);
        // }

        // Player.InventorySystem.AddCurrency(_currentLootTable.gold);
        // Player.AttributeSystem.IncreaseExp(_currentLootTable.exp);
        // if(Player.AttributeSystem.Level.LevelUp) IsLevelUp = true;
        // Player.AttributeSystem.LevelUp();
    }

    public void ShowEndButton(bool visible)
    {
        Presenter.SetEndButtonVisibility(visible);
        Presenter.SetEndButtonEnabled(visible);
    }

    public IEnumerator FadeInLevelUp()
    {
        //Add TryGetValue from Depot where you will handle Logging errors!
        // LootTable _currentLootTable = Depot.LootTables.Find(table => table.battleId == _currentBattle.id);
        // if(_currentLootTable == null) 
        //     Debug.LogError("ERROR! Enemy with id " + _currentEnemy.id + "(" + _currentEnemy.name + ") has NO loot table!!!");

        // yield return Presenter.FadeInLevelUp(Player.AttributeSystem.Attributes.Attributes);
        yield return null;
    }

    public void SetLevelUpVisibility(bool visible)
    {
        Presenter.SetLevelUpVisibility(visible);
    }

    public IEnumerator Shake(float multiplier = 1)
    {
        yield return Presenter.Shake(multiplier);
    }

    public void ConsumeItem(int id)
    {
        ConsoleProDebug.LogToFilter($"Player Consumed item by id {id}", "BattleSystem");
        // int itemId = Player.InventorySystem.InventoryModel.Get(id);
        
        // CurrentBattler.Busy = false;

        // AddCommand(new ConsumeItemCommand(this, CurrentBattler, OtherBattler, itemId));
        Player.InventorySystem.InventoryModel.Take(id, 1);
        Presenter.SetInventoryVisibility(false);
    }
    
    #region BattleState
    public IEnumerator Won()
    {
        yield return Presenter.Won();
    }

    public IEnumerator Lost()
    {
        yield return Presenter.Lost();
    }

    public IEnumerator Loop()
    {
        _isRunning = true;
        Wait = false;
        _battleFSM.SetState(EBattleState.Start);

        while(_isRunning)
        {
            if(!Wait) yield return StartCoroutine(_battleFSM.Update());
            else yield return null;
        }
    }
    #endregion
}
