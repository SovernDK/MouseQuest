using System.Collections.Generic;
using System.Linq;
using Atlas.DB;
using Atlas.Enums;
using Atlas.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace Atlas.AI 
{
    public class EnemyMoveDecider
    {
        private EnemyBattler _battler;
        private List<AvailableMove> _availableNormalMoves;
        private List<AvailableMove> _availableQuickMoves;
        private List<AvailableMove> _allMoves;

        public List<AvailableMove> AllMoves { get => _allMoves; set => _allMoves = value; }
        public List<AvailableMove> AvailableMoves { get => _availableNormalMoves; set => _availableNormalMoves = value; }
        public List<AvailableMove> AvailableQuickMoves { get => _availableQuickMoves; set => _availableQuickMoves = value; }

        public EnemyMoveDecider(EnemyBattler battler)
        {
            _availableNormalMoves = new List<AvailableMove>();
            _availableQuickMoves = new List<AvailableMove>();
            _allMoves = new List<AvailableMove>();

            _battler = battler;
        }

        public void Set(Enemy enemy)
        {
            enemy.moves.ForEach(move => {
                if(move.enabled)
                {
                    AvailableMove availableMove = new AvailableMove()
                    {
                        moveId = move.moveId,
                        priority = move.priority,
                        conditions = move.attributeConditions,
                        spellName = move.spell?.spell.name ?? string.Empty,
                        enemies = move.enemies.Select(e => e.name).ToList(),
                        turnCondition = move.turnCondition,
                        stateConditions = move.stateConditions,
                        weightGain = move.weightGain
                    };

                    _allMoves.Add(availableMove);
                }
            });
        }

        public (EEnemyMove, string, string) DecideNormalMove()
        {
            _availableNormalMoves.ForEach(move => move.weight = Mathf.Clamp(move.weight - 1, 0, float.MaxValue));
            _availableNormalMoves.Clear();

            //Exclude Quick Moves!
            _allMoves.ForEach(move => 
            {
                if(move.Check(_battler))
                {
                    _availableNormalMoves.Add(move);
                }
            });

            //Add some randomness to priority so enemy wont be constantly switching moves back and forth
            _availableNormalMoves.ForEach(move => move.priority += Random.Range(Config.Instance.priorityRandomness.x, Config.Instance.priorityRandomness.y));
            _availableNormalMoves.Sort((obj1, obj2) => (obj2.priority - obj2.weight).CompareTo(obj1.priority - obj1.weight));
            AvailableMove decided = _availableNormalMoves[0];

            if(decided.weight >= Config.Instance.weightReset) decided.weight = 0;
            else decided.weight += decided.weightGain > 0 ? decided.weightGain : Config.Instance.weightIncrease;

            int result = Mathf.Clamp((int) decided.moveId, 0, System.Enum.GetValues(typeof(EEnemyMove)).Length);

            List<string> transformationTargets = Util.RandomlySortList(decided.enemies);  
            string enemy = (transformationTargets.Count > 0) ? transformationTargets[0] : "";

            return ((EEnemyMove) result, decided.spellName, enemy);
        }

        public string DecideQuickMove()
        {
            _availableQuickMoves.Clear();
            _allMoves.ForEach(move => 
            {
                if(move.Check(_battler))
                {
                    if(move.moveId == EEnemyMove.Spellcast && Database.Instance.GetSpell(move.spellName).castType == ECastType.Quick)
                        _availableQuickMoves.Add(move);
                }
            });

            if(_availableQuickMoves.Count == 0) return "none";

            //Add some randomness to priority so enemy wont be constantly switching moves back and forth
            _availableQuickMoves.ForEach(move => move.priorityModifier = Random.Range(Config.Instance.priorityRandomness.x, Config.Instance.priorityRandomness.y));
            _availableQuickMoves.Sort((obj1, obj2) => (obj2.priority + obj2.priorityModifier - obj2.weight).CompareTo(obj1.priority + obj1.priorityModifier - obj1.weight));
            AvailableMove decided = _availableQuickMoves[0];

            if(decided.weight >= Config.Instance.weightReset) decided.weight = 0;
            else decided.weight += decided.weightGain > 0 ? decided.weightGain : Config.Instance.weightIncrease;

            int result = Mathf.Clamp((int) decided.moveId, 0, System.Enum.GetValues(typeof(EEnemyMove)).Length);
            return decided.spellName;
        }
    }

    public class AvailableMove
    {
        public EEnemyMove moveId;
        public int priorityModifier = 0;
        public int priority;
        public float weight;
        public float weightGain;
        public string spellName;
        public List<string> enemies;
        public List<AttributeConditions> conditions;
        public List<StateConditions> stateConditions;
        public TurnConditions turnCondition;

        public bool Check(EnemyBattler enemy)
        {
            foreach(AttributeConditions con in conditions)
            {
                int maxValue = enemy.Attributes.GetMaxValue(con.attributeId);
                int value = enemy.Attributes.GetValue(con.attributeId);

                switch(con.condition)
                {
                    case ECondition.Equal:
                        if(value != CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                    case ECondition.LessThen:
                        if(value > CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                    case ECondition.LessOrEqual:
                        if(value >= CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                    case ECondition.MoreThen:
                        if(value < CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                    case ECondition.MoreOrEqual:
                        if(value <= CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                    default:
                        if(value != CalculateValue(con.valueType, con.value, maxValue)) return false; 
                        break;
                }   
            }

            foreach(StateConditions con in stateConditions)
            {
                if(con.target == ETarget.User)
                {
                    bool check = enemy.Attributes.GetState(con.state.state.name) != null;
                    if(check != con.exist) return false;
                }
            }

            if(turnCondition.enable == false) return true;

            switch(turnCondition.after)
            {
                case true:
                    if(enemy.TurnCounter < turnCondition.turn) return false;
                    break;    
                case false:
                    if(enemy.TurnCounter > turnCondition.turn) return false;
                    break;    
            }
            
            return true;
        }

        public int CalculateValue(EValueType valueType, int value, int max)
        {
            if(valueType == EValueType.Percent)
            {
                return max * value / 100;
            }    
            else return value;
        }
    }
}