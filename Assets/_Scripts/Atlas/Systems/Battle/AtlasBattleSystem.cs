using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atlas.AI;
using Atlas.Core;
using Atlas.DB;
using Atlas.Effects;
using Atlas.Enums;
using Atlas.Player;
using Atlas.Presenters;
using Atlas.Utility;
using DG.Tweening;
using NodeCanvas.StateMachines;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Atlas.Systems
{
    public class AtlasBattleSystem : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _battleScenes; 

        private FSMOwner _fsm;
        
        private PlayerBattler _playerBattler;
        private EnemyBattler _enemyBattler;
        private Enemy _currentEnemy;
        private Battler[] _battlers;
        private EnemyBattler[] _enemies;
        private Queue<Battler> _combatantOrder;
        private Dictionary<Battler, BattleCommand> _commandsByBattler;
        private Dictionary<Battler, BattleCommand> _quickCommandsByBattler;

        private Battler _acting;
        private Battler _other;
        private int _turnCount;
        private bool _end;
        private bool _won;
        private bool _debug;

        [Inject]
        private AtlasBattlePresenter _presenter;
        [Inject]
        private PlayerSystem _playerSystem;
        [Inject]
        private Config _config;
        [Inject]
        private ProgressSystem _progression;
        [Inject]
        private ShopSystem _shop;

        [Inject]
        public EffectsSystem Effects { get; }
        public Config Config { get => _config; set => _config = value; }

        #region Events
        [FoldoutGroup("Battle Events")]
        public UnityEvent BattleStart;

        [FoldoutGroup("Battle Events")]
        public UnityEvent StartTurnEnter;
        
        [FoldoutGroup("Battle Events")]
        public UnityEvent PlayerTurn;
        
        [FoldoutGroup("Battle Events")]
        public UnityEvent EnemyTurn;
        
        [FoldoutGroup("Battle Events")]
        public UnityEvent ExecutionTurn;
        
        [FoldoutGroup("Battle Events")]
        public UnityEvent EndTurn;

        [FoldoutGroup("Battle Events")]
        public UnityEvent BattleEnded;

        [FoldoutGroup("Battle Events")]
        public UnityEvent<Queue<Battler>> TunOrderDecided;

        [FoldoutGroup("Loot Events")]
        public UnityEvent<List<Loot>> GiveLoot;

        [FoldoutGroup("Loot Events")]
        public UnityEvent StartLoot;

        [FoldoutGroup("Loot Events")]
        public UnityEvent EndLoot;

        [FoldoutGroup("Reward Events")]
        public UnityEvent<List<Spell>> GiveRewards;

        [FoldoutGroup("Reward Events")]
        public UnityEvent StartReward;

        [FoldoutGroup("Reward Events")]
        public UnityEvent EndReward;
        
        [FoldoutGroup("Rest Events")]
        public UnityEvent EnterRest;
        #endregion
        
        private void Awake() 
        {
            Initialize();
            _progression.Initialize();
        }

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                _debug = !_debug;
            }
        }

        public void Initialize()
        {
            _presenter.System = this;

            _combatantOrder = new Queue<Battler>();
            _commandsByBattler = new Dictionary<Battler, BattleCommand>();
            _quickCommandsByBattler = new Dictionary<Battler, BattleCommand>();

            _fsm = GetComponent<FSMOwner>();
            SetState(EAtlasBattleState.Rest);
            
            _playerBattler = FindAnyObjectByType<PlayerBattler>();
            _enemyBattler = FindAnyObjectByType<EnemyBattler>();

            _battlers = new Battler[2];

            _playerBattler.Initialize(_playerSystem);
            _battlers[0] = _playerBattler;

            _enemyBattler.Initialize();
            _battlers[1] = _enemyBattler;

            _enemies = new EnemyBattler[1];
            _enemies[0] = _enemyBattler;
        }

        #region BattleStates
        public void RunBattle()
        {
            _shop.Close();
            SetState(EAtlasBattleState.BattleStart);
        }

        public IEnumerator StartBattle()
        {
            yield return Gamemaster.Instance.TransitionView.FadeIn().WaitForCompletion();
            
            SoundEffect sfx = new SoundEffect("MetalDoor_Open_Wet", Vector3.zero)
                            .Build() as SoundEffect;

            Effects.AddEffect(sfx);

            yield return new WaitForSeconds(2f);
            
            _turnCount = 0;
            _presenter.StartBattle();

            _currentEnemy = _progression.GetNextEnemy();

            _battleScenes.ForEach(scene => 
            {
                if(scene.name.Equals(_currentEnemy.battleBackScene)) scene.SetActive(true);
                else scene.SetActive(false);
            });

            SoundEffect footsteps = new SoundEffect("Footsep_FlatStone_Wet", Vector3.zero)
                            .Build() as SoundEffect;

            Effects.AddEffect(footsteps);

            _enemyBattler.CreateBattlerFromEnemy(_currentEnemy);
            _playerBattler.SetCharacter(_playerSystem.PlayerCharacter);
            _enemyBattler.OnBattleStart();
            _playerBattler.OnBattleStart();
            
            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
            
            BattleStart.Invoke();
            
            yield return Gamemaster.Instance.TransitionView.FadeOut().WaitForCompletion();
            SetState(EAtlasBattleState.TurnStart);
        }

        public IEnumerator TurnStarted()
        {
            StartTurnEnter.Invoke();

            _turnCount++;
            _playerBattler.TurnCounter = _turnCount;
            _enemyBattler.TurnCounter = _turnCount;

            _presenter.StartTurn();
            _playerBattler.OnTurnStart();
            _enemyBattler.OnTurnStart();

            TurnOrder();

            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);

            SetState(EAtlasBattleState.Decision);
        }

        public IEnumerator Dialogue(bool fromEnemy)
        {
            if(fromEnemy)
            {
                _currentEnemy.sequences.ForEach(sequence =>
                {
                    if(_turnCount == sequence.turn)
                    {
                        DialogueManager.StartConversation(sequence.conversation);
                    }
                });
            }

            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
        }

        public IEnumerator PlayerDeciding()
        {
            _fsm.SetExposedParameterValue("waitingForPlayer", true);
            _presenter.PlayerTurn();

            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
            _presenter.EnableCommands(true);
            PlayerTurn.Invoke();

            _acting = _playerBattler;
            _other = _enemyBattler;
        } 

        public IEnumerator EnemiesDeciding()
        {
            EnemyTurn.Invoke();
            _presenter.EnemiesTurn();

            _acting = _enemyBattler;
            _other = _playerBattler;

            foreach (EnemyBattler enemy in _enemies)
            {
                yield return enemy.MakeMove();
            }

            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
        }

        public IEnumerator ExecuteCommands()
        {
            yield return _playerBattler.OnExecutionStarted();
            yield return _enemyBattler.OnExecutionStarted();

            SetState(EAtlasBattleState.Execution);

            while(_combatantOrder.Count > 0)
            {
                Battler acting = _combatantOrder.Dequeue();
                yield return acting.PlayActing();

                if(acting.QuickCommand != null)
                    yield return acting.QuickCommand.Execute();

                yield return acting.NormalCommand.Execute();

                if(!_acting.Alive || !_other.Alive)
                {
                    ConsoleProDebug.LogToFilter("Someone Died", "BattleFSM");
                    _end = true;
                    break;
                }

                yield return acting.PlayEndActing();
                yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
            }
            
            if(!_end) 
            {
                SetState(EAtlasBattleState.TurnStart);
            }
            else
            {
                _end = false;
                SetState(EAtlasBattleState.BattleEnd);

                if(_playerBattler.Alive)
                    SetState(EAtlasBattleState.Reward);
                else
                    FindAnyObjectByType<GameStateSystem>().SetState(EGameState.GameOver);
                    // SetState(EAtlasBattleState.Rest);
                // if(_playerBattler.Alive) _won = true;
                // else _won = false;
            }
        }

        public IEnumerator TurnEnd()
        {
            EndTurn.Invoke();

            _commandsByBattler.Clear();
            
            _playerBattler.OnTurnEnd();
            _enemyBattler.OnTurnEnd();

            yield return new WaitForSeconds(Config.Instance.battleStatesInterval);
        }

        public IEnumerator Loot()
        {
            StartLoot.Invoke();
            GiveLoot.Invoke(_currentEnemy.loot);

            yield return new WaitForSeconds(2f);
            
            _currentEnemy.loot.ForEach(loot => _playerSystem.GetComponent<InventorySystem>().AddItem(loot.prototype.item.name, loot.amount));
            EndLoot.Invoke();
        }

        public IEnumerator Reward()
        {
            _fsm.SetExposedParameterValue("waitingForPlayer", true);

            StartReward.Invoke();
            GiveRewards.Invoke(GetRandomReward());
            yield return null;
        }

        public void RerollRewards()
        {
            GiveRewards.Invoke(GetRandomReward());
        }

        public void ChooseSpellReward(string spellId)
        {
            // Debug.Log($"Player choose spell {spellId}");
            _fsm.SetExposedParameterValue("waitingForPlayer", false);
            _playerSystem.GetComponent<SpellSystem>().UnlockSpell(spellId);
            EndReward.Invoke();
        }

        public IEnumerator EndBattle()
        {
            _playerSystem.GetComponent<InventorySystem>().AddCurrency(_currentEnemy.reward);
            BattleEnded.Invoke();
            _presenter.EndBattle();

            yield return Gamemaster.Instance.TransitionView.FadeIn().WaitForCompletion();

            _presenter.EnableCommands(true);
        }
        #endregion

        #region RestStates
        public void Rest()
        {
            Gamemaster.Instance.TransitionView.FadeOut();

            SetState(EAtlasBattleState.Rest);
            EnterRest.Invoke();
        }

        public void Exit()
        {
            Gamemaster.Instance.TransitionView.FadeIn();

            _fsm.enabled = false;
            FindAnyObjectByType<GameStateSystem>().SetState(EGameState.GameOver);
        }
        #endregion

        // public void TurnOrder()
        // {
        //     _combatantOrder.Clear(); 

        //     Dictionary<Battler, float> battlerValues = new Dictionary<Battler, float>();
        //     float playerSpeed = _battlers[0].Attributes.GetValue(EAttribute.Speed);

        //     foreach (Battler battler in _battlers)
        //     {
        //         float battlerSpeed = battler.Attributes.GetValue(EAttribute.Speed);
        //         float totalSpeed = playerSpeed + battlerSpeed;

        //         // Calculate the probability or priority value
        //         float probabilityValue = playerSpeed > 0 ? battlerSpeed / totalSpeed : 0;

        //         // Assign a random value scaled by the probability for tie-breaking randomness
        //         float assignedValue = probabilityValue + Random.Range(0f, 0.1f);

        //         battlerValues[battler] = assignedValue;
        //     }

        //     // Sort the battlers by their assigned value in descending order
        //     foreach (var battler in battlerValues.OrderByDescending(kvp => kvp.Value))
        //     {
        //         _combatantOrder.Enqueue(battler.Key);
        //     }

        //     Debug.Log($"Combatant count {_combatantOrder.Count}");

        //     TunOrderDecided.Invoke(_combatantOrder);
        // }

        public void TurnOrder()
        {
            _combatantOrder.Clear();

            Dictionary<Battler, float> battlerProbabilities = new Dictionary<Battler, float>();
            float totalSpeed = _battlers.Sum(battler => battler.Attributes.GetValue(EAttribute.Speed));

            // Calculate the probability for each battler based on their speed
            foreach (Battler battler in _battlers)
            {
                float battlerSpeed = battler.Attributes.GetValue(EAttribute.Speed);
                float probability = totalSpeed > 0 ? battlerSpeed / totalSpeed : 0;
                battlerProbabilities[battler] = probability;
            }

            // Create a list of battlers to be ordered
            List<Battler> remainingBattlers = new List<Battler>(_battlers);

            // Perform weighted random selection to determine the order
            while (remainingBattlers.Count > 0)
            {
                float totalWeight = remainingBattlers.Sum(battler => battlerProbabilities[battler]);
                float randomValue = Random.Range(0f, totalWeight);
                float cumulativeWeight = 0f;

                foreach (Battler battler in remainingBattlers)
                {
                    cumulativeWeight += battlerProbabilities[battler];
                    if (randomValue <= cumulativeWeight)
                    {
                        _combatantOrder.Enqueue(battler);
                        remainingBattlers.Remove(battler);
                        break;
                    }
                }
            }

            Debug.Log($"Combatant count {_combatantOrder.Count}");

            TunOrderDecided.Invoke(_combatantOrder);
        }

        public void Transformation(Enemy enemy, bool transferHp)
        {
            int hpValue = _enemyBattler.Attributes.GetValue(EAttribute.Hitpoints);
            _enemyBattler.CreateBattlerFromEnemy(enemy);
            
            if(transferHp)
                _enemyBattler.Attributes.SetAttributeValue(EAttribute.Hitpoints, hpValue);
        }

        public IEnumerator ShowNotification(string text)
        {
            yield return _presenter.ShowNotification(text);
        }

        #region Commands
        public void AttackCommand()
        {
            _acting.AddNormalCommand(new AttackBattleCommand(this, _acting, _other));
            _fsm.SetExposedParameterValue("waitingForPlayer", false);
        }

        public void RiskyAttackCommand()
        {
            _acting.AddNormalCommand(new RiskyAttackBattleCommand(this, _acting, _other));
            _fsm.SetExposedParameterValue("waitingForPlayer", false);
        }

        public void SpellCommand(string spellId)
        {
            switch(Database.Instance.GetSpell(spellId).castType)
            {
                case ECastType.Normal:
                    _acting.AddNormalCommand(new CastSpellCommand(this, _acting, _other, spellId));
                    _fsm.SetExposedParameterValue("waitingForPlayer", false);
                    break;
                case ECastType.Quick:
                    _acting.AddQuickCommand(new CastSpellCommand(this, _acting, _other, spellId));
                    break;
            }
        }

        public void TransformationCommand(Enemy enemy, bool transferHp)
        {
            _acting.AddNormalCommand(new TransformationBattleCommand(this, _acting, _other, enemy, transferHp));
            _fsm.SetExposedParameterValue("waitingForPlayer", false);
        }

        public void ConsumeItem(DB.Item item)
        {
            _acting.AddNormalCommand(new ConsumeItemCommand(this, _acting, _other, item.name));
            _fsm.SetExposedParameterValue("waitingForPlayer", false);
        }
        #endregion

        public void SetState(EAtlasBattleState state)
        {
            _fsm.SetExposedParameterValue("state", state);
        }

        public void SetState(int stateId)
        {
            _fsm.SetExposedParameterValue("state", (EAtlasBattleState) stateId);
        }

        #region Debug
        private void OnGUI() 
        {
            if(!_debug) return;
            if(_enemyBattler.Attributes == null) return;

            // Create a GUIStyle for the background
            GUIStyle backgroundStyle = new GUIStyle();
            backgroundStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f)); // Semi-transparent black

            // Draw the background
            GUI.Box(new Rect(10, 10, 1080, 600), "", backgroundStyle);

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUI.Label(new Rect(150, 15, 200, 20), $"{_fsm.GetExposedParameterValue<EAtlasBattleState>("state")}");
            GUI.Label(new Rect(150, 30, 200, 20), $"Turn Count: {_turnCount}");

            float posX = 250;
            float posY = 15;

            GUI.Label(new Rect(posX, posY, 200, 20), "PLAYER");
            posY += 15;

            foreach(CharacterSheet.Attribute attr in _playerBattler.Attributes.Attributes)
            {
                if(attr.VariableAttribute)
                {
                    string value = (attr.Value >= attr.MaxValue) ? $"<color=green>{attr.Value}</color>" : $"<color=red>{attr.Value}</color>";
                    GUI.Label(new Rect(posX, posY, 200, 20), $"{attr.Id} {value} ({attr.MaxValue})", style);
                }
                else
                {
                    string value = (attr.Value > attr.BaseValue) ? $"<color=green>{attr.Value}</color>" : $"<color=red>{attr.Value}</color>";
                    GUI.Label(new Rect(posX, posY, 200, 20), $"{attr.Id} {value} ({attr.BaseValue})", style);
                }

                posY += 15;
            }

            posY += 15;     
            GUI.Label(new Rect(posX, posY, 200, 20), "PLAYER MODIFIERS");
            posY += 15;
            foreach(CharacterSheet.Attribute attr in _playerBattler.Attributes.Attributes)
            {
                foreach(KeyValuePair<string, CharacterSheet.AttributeModifier> mod in attr.Modifiers)
                {
                    string value = $"{mod.Key} ({attr.Id} + {mod.Value.Value})";
                    if(mod.Value.IsTimeLimited)
                        value = $"{mod.Key} {mod.Value.Value} ({mod.Value.TurnCount})";
                        
                    GUI.Label(new Rect(posX, posY, 500, 20), value, style);
                    posY += 15;
                }
            }

            foreach(CharacterSheet.Resistance res in _playerBattler.Attributes.Resistance)
            {
                foreach(KeyValuePair<string, CharacterSheet.ResistanceModifier> mod in res.Modifiers)
                {
                    string value = $"{mod.Key} ({res.Id} + {mod.Value.Value})";
                    if(mod.Value.IsTimeLimited)
                        value = $"{mod.Key} {mod.Value.Value} ({mod.Value.TurnCount})";
                        
                    GUI.Label(new Rect(posX, posY, 500, 20), value, style);
                    posY += 15;
                }
            }

            GUI.Label(new Rect(posX, posY, 500, 20), $"Base Element: {_playerBattler.BaseAtkElement}", style);    

            posY += 15;     
            GUI.Label(new Rect(posX, posY, 200, 20), "PLAYER STATES");
            posY += 15;
            foreach(CharacterSheet.State state in _playerBattler.Attributes.States)
            {
                GUI.Label(new Rect(posX, posY, 500, 20), $"{state.Id} ({state.TurnsLeft})", style);
                posY += 15;
            }

            posY += 15;     
            GUI.Label(new Rect(posX, posY, 200, 20), "PLAYER RES");
            posY += 15;
            foreach(CharacterSheet.Resistance res in _playerBattler.Attributes.Resistance)
            {
                GUI.Label(new Rect(posX, posY, 500, 20), $"{res.Id} ({res.Value})", style);
                posY += 15;
            }

            // ENEMIES
            posX = 900;
            GUI.Label(new Rect(posX, 15, 200, 20), $"ENEMY: {_enemyBattler.Name}");
            posY = 30;
            foreach(CharacterSheet.Attribute attr in _enemyBattler.Attributes.Attributes)
            {
                if(attr.VariableAttribute)
                {
                    string value = (attr.Value >= attr.MaxValue) ? $"<color=green>{attr.Value}</color>" : $"<color=red>{attr.Value}</color>";
                    GUI.Label(new Rect(posX, posY, 200, 20), $"{attr.Id} {value} ({attr.MaxValue})", style);
                }
                else
                {
                    string value = (attr.Value >= attr.BaseValue) ? $"<color=green>{attr.Value}</color>" : $"<color=red>{attr.Value}</color>";
                    GUI.Label(new Rect(posX, posY, 200, 20), $"{attr.Id} {value} ({attr.BaseValue})", style);
                }
                posY += 15;
            }
            
            posY += 15;
            //ENEMY MOVES
            GUI.Label(new Rect(posX, posY, 200, 20), "MOVES");
            posY += 15;

            foreach(AvailableMove move in _enemyBattler.EnemyMoveDecider.AvailableMoves)
            {
                string text = $"id: {move.moveId} (<color=green>{move.priority - move.weight}</color>) [p: {move.priority}] [w: {move.weight}]";
                if(move.moveId == EEnemyMove.Spellcast)
                    text = $"id: {move.spellName} (<color=green>{move.priority - move.weight}</color>) [p: {move.priority}] [w: {move.weight}]";
                    
                GUI.Label(new Rect(posX, posY, 200, 20), text, style);
                posY += 15;
            }

            posY += 15;     
            GUI.Label(new Rect(posX, posY, 200, 20), "ENEMY RES");
            posY += 15;
            foreach(CharacterSheet.Resistance res in _enemyBattler.Attributes.Resistance)
            {
                GUI.Label(new Rect(posX, posY, 500, 20), $"{res.Id} ({res.BaseValue})", style);
                posY += 15;
            }
        }

        // Helper function to create a texture for the background
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for(int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        #endregion

        private List<Spell> GetRandomReward()
        {
            List<Spell> commonSpells = Database.Instance.GetAllSpells().FindAll(s => s.playerCharacter == null && 
                    !_playerBattler.Spellbook.Get(s.name).Unlocked);

            List<Spell> classSpells = Database.Instance.GetAllSpells().FindAll(s => s.playerCharacter != null && 
                    s.playerCharacter.playerCharacter.name.Equals(_playerSystem.PlayerCharacter.name) && //Database.Instance.GetCharacter(ES3.Load<int>("character")).name
                    !_playerBattler.Spellbook.Get(s.name).Unlocked);

            commonSpells = Util.RandomlySortList(commonSpells);
            classSpells = Util.RandomlySortList(classSpells);

            Spell commonSpell1 = Database.Instance.GetSpell(commonSpells[0].name);
            Spell commonSpell2 = Database.Instance.GetSpell(commonSpells[1].name);
            Spell classSpell = (classSpells.Count > 0)  ? Database.Instance.GetSpell(classSpells[0].name) : Database.Instance.GetSpell(commonSpells[2].name);

            return new List<Spell> { commonSpell1, commonSpell2, classSpell};
        }

        private void OnDisable() 
        {
            BattleStart.RemoveAllListeners();
            StartTurnEnter.RemoveAllListeners();
            PlayerTurn.RemoveAllListeners();
            EnemyTurn.RemoveAllListeners();
            ExecutionTurn.RemoveAllListeners();
            EndTurn.RemoveAllListeners();
            TunOrderDecided.RemoveAllListeners();
            EnterRest.RemoveAllListeners();
        }
    }
    
    public enum EAtlasBattleState
    {
        BattleStart, TurnStart, Decision, Execution, TurnEnd, BattleEnd, Reward, Rest, Dialogue
    }
}
