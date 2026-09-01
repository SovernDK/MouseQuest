using System;
using System.Collections.Generic;
using System.Linq;
using Atlas.DB;
using Atlas.Enums;
using Atlas.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace CharacterSheet
{
    [Serializable]
    public class AttributesModel : IModel
    {
        [SerializeField]
        private Attribute[] _attributes;
        [SerializeField]
        private Resistance[] _resistance;
        [SerializeField]
        private List<State> _states;
        [SerializeField]
        private EElement _baseElement = EElement.None;
        private AttributeModifier _elementModifier;

        private Vector2Int _range;
        private Vector2Int _hpRange;

        public Attribute[] Attributes { get => _attributes; set => _attributes = value; }
        public Resistance[] Resistance { get => _resistance; set => _resistance = value; }
        public List<State> States { get => _states; set => _states = value; }
        public EElement BaseElement 
        { 
            get 
            {
                if(_elementModifier != null) return (EElement) _elementModifier.Value;
                else return _baseElement;
            }
            set => _baseElement = value; 
        }

        public AttributeModifier ElementModifier { get => _elementModifier; set => _elementModifier = value; }

        public AttributesModel()
        {
            _range = Config.Instance.attributeValueLimits;
            _hpRange = Config.Instance.hpValuesLimit;

            Array enums = Enum.GetValues(typeof(EAttribute));
            _attributes = new Attribute[enums.Length];

            _attributes[(int) EAttribute.Hitpoints] = new Attribute() { Id = EAttribute.Hitpoints, Range = _hpRange, BaseMaxValue = _hpRange.y, VariableAttribute = true };
            _attributes[(int) EAttribute.Attack] = new Attribute() { Id = EAttribute.Attack, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };
            _attributes[(int) EAttribute.Defence] = new Attribute() { Id = EAttribute.Defence, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };
            _attributes[(int) EAttribute.MagicAttack] = new Attribute() { Id = EAttribute.MagicAttack, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };
            _attributes[(int) EAttribute.MagicDefence] = new Attribute() { Id = EAttribute.MagicDefence, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };
            _attributes[(int) EAttribute.Speed] = new Attribute() { Id = EAttribute.Speed, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };
            _attributes[(int) EAttribute.CriticalHit] = new Attribute() { Id = EAttribute.CriticalHit, Range = _range, BaseMaxValue = _range.y, VariableAttribute = false };

            Array resistanceEnums = Enum.GetValues(typeof(EElement));
            _resistance = new Resistance[resistanceEnums.Length];

            for(int i = 0; i < resistanceEnums.Length; i++)
            {
                _resistance[i] = new Resistance()
                {
                    Id = (EElement) i,
                    BaseValue = 0,
                };
            }

            _states = new List<State>();
        }

        public AttributesModel(Attribute[] attributes, Resistance[] resistance, Vector2Int range)
        {
            _attributes = attributes;
            _resistance = resistance;
            _range = range;
        }

        // [Button("Load Attributes")]
        private void LoadAttributes()
        {
            Array enums = Enum.GetValues(typeof(EAttribute));
            _attributes = new Attribute[enums.Length];

            for(int i = 0; i < enums.Length; i++)
            {
                _attributes[i] = new Attribute()
                {
                    Id = (EAttribute) i,
                    BaseValue = 1,
                    BaseMaxValue = 999,
                    VariableAttribute = i == 0 ? true : false
                };
            }
        }

        // [Button("Load Resistance")]
        private void LoadResistance()
        {
            Array resistanceEnums = Enum.GetValues(typeof(EElement));
            _resistance = new Resistance[resistanceEnums.Length];

            for(int i = 0; i < resistanceEnums.Length; i++)
            {
                _resistance[i] = new Resistance()
                {
                    // Id = i,
                    Id = (EElement) i,
                    BaseValue = 0,
                };
            }
        }

        public void LoadModelValues(AttributesModel data)
        {
            _attributes = data.Attributes;
            _resistance = data.Resistance;
        }

        #region Attribute Modification
        public void IncreaseAttribute(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            if(attribute.VariableAttribute)
                attribute.BaseMaxValue = Mathf.Clamp(attribute.BaseMaxValue + value, attribute.Range.x, attribute.Range.y);
            else
                attribute.BaseValue = Mathf.Clamp(attribute.BaseValue + value, attribute.Range.x, attribute.MaxValue);
        }

        public void DecreaseAttribute(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            if(attribute.VariableAttribute)
                attribute.BaseMaxValue = Mathf.Clamp(attribute.BaseMaxValue - value, attribute.Range.x, attribute.Range.y);
            else
                attribute.BaseValue = Mathf.Clamp(attribute.BaseValue - value, attribute.Range.x, attribute.MaxValue);
        }

        public void SetAttribute(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            if(attribute.VariableAttribute)
                attribute.BaseMaxValue = Mathf.Clamp(value, attribute.Range.x, attribute.Range.y);
            else
                attribute.BaseValue = Mathf.Clamp(value, attribute.Range.x, attribute.MaxValue);
        }
        #endregion

        #region Attribute Value Modification
        public void SetAttributeValue(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            attribute.BaseValue = Mathf.Clamp(value,  attribute.Range.x, attribute.MaxValue);
        }

        public void IncreaseAttributeValue(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            attribute.BaseValue = Mathf.Clamp(attribute.Value + value, _range.x, attribute.MaxValue);
            Debug.Log($"HP type {type}: value = {value}, baseValue = {attribute.BaseValue}, maxValue = {attribute.MaxValue}");
        }

        public void DecreaseAttributeValue(EAttribute type, int value)
        {
            Attribute attribute = _attributes[(int) type];
            attribute.BaseValue = Mathf.Clamp(attribute.BaseValue - value, attribute.Range.x, attribute.MaxValue);
        }
        #endregion

        public void Heal(int value)
        {
            SetAttributeValue(EAttribute.Hitpoints, GetValue(EAttribute.Hitpoints) + value);
        }

        public void AddModifier(EAttribute type, string key, int value, bool isTimeLimited = false, int turnCount = -1)
        {
            AttributeModifier newModifier = new AttributeModifier()
            {
                Value = value,
                IsTimeLimited = isTimeLimited,
                TurnCount = turnCount
            };

            if(_attributes[(int) type].Modifiers.TryGetValue(key, out AttributeModifier modifier))
                modifier.Value = value;
            else 
                _attributes[(int) type].Modifiers.Add(key, newModifier);
        }

        public void AddModifier(EElement type, string key, int value, bool isTimeLimited = false, int turnCount = -1)
        {
            ResistanceModifier newModifier = new ResistanceModifier()
            {
                Value = value,
                IsTimeLimited = isTimeLimited,
                TurnCount = turnCount
            };

            if(_resistance[(int) type].Modifiers.TryGetValue(key, out ResistanceModifier modifier))
            {
                modifier.Value = value;
                modifier.TurnCount = turnCount;
            }
            else 
                _resistance[(int) type].Modifiers.Add(key, newModifier);
        }

        public void AddElementModifier(EElement type, bool isTimeLimited = false, int turnCount = -1)
        {
            AttributeModifier newModifier = new AttributeModifier()
            {
                Value = (int) type,
                IsTimeLimited = isTimeLimited,
                TurnCount = turnCount
            };

            _elementModifier = newModifier;
        }

        public void AddMaxModifier(EAttribute type, string key, int value, bool isTimeLimited = false, int turnCount = -1)
        {
            AttributeModifier newModifier = new AttributeModifier()
            {
                Value = value,
                IsTimeLimited = isTimeLimited,
                TurnCount = turnCount
            };

            if(_attributes[(int) type].MaxValueModifiers.TryGetValue(key, out AttributeModifier modifier))
                modifier.Value = value;
            else 
                _attributes[(int) type].MaxValueModifiers.Add(key, newModifier);
        }

        public void RemoveAllEquipmentModifiers()
        {
            List<string> modifiersToRemove = new List<string>();
            foreach(Attribute attr in _attributes)
            {
                foreach(string key in attr.Modifiers.Keys)
                {
                    if(key.StartsWith("equipment_")) modifiersToRemove.Add(key);
                }

                foreach(string key in modifiersToRemove)
                {
                    attr.Modifiers.Remove(key);
                }

                modifiersToRemove.Clear();
            }
        }

        public void CountModifiersDown()
        {
            //ELEMENT
            if(_elementModifier != null && _elementModifier.IsTimeLimited)
            {
                _elementModifier.TurnCount--;
                if(_elementModifier.TurnCount <= 0)
                {
                    _elementModifier = null;
                }
            }
            //

            //ATTRIBUTES
            List<string> modifiersToRemove = new List<string>();

            foreach(Attribute attr in _attributes)
            {
                foreach(KeyValuePair<string, AttributeModifier> modifier in attr.Modifiers)
                {
                    if(!modifier.Value.IsTimeLimited) continue;

                    modifier.Value.TurnCount--;
                    
                    Debug.Log("Modifier to remove: " + modifier.Key + " turn count: " + modifier.Value.TurnCount);
                    if(modifier.Value.TurnCount <= 0)
                    {
                        modifiersToRemove.Add(modifier.Key);
                    }
                }

                foreach(string key in modifiersToRemove)
                {
                    attr.Modifiers.Remove(key);
                }

                modifiersToRemove.Clear();
            }
            
            //RESISTANCES
            foreach(Resistance res in _resistance)
            {
                foreach(KeyValuePair<string, ResistanceModifier> modifier in res.Modifiers)
                {
                    if(!modifier.Value.IsTimeLimited) continue;

                    modifier.Value.TurnCount--;
                    
                    Debug.Log("Modifier to remove: " + modifier.Key + " turn count: " + modifier.Value.TurnCount);
                    if(modifier.Value.TurnCount <= 0)
                    {
                        modifiersToRemove.Add(modifier.Key);
                    }
                }

                foreach(string key in modifiersToRemove)
                {
                    res.Modifiers.Remove(key);
                }

                modifiersToRemove.Clear();
            }
        }

        public void CountStatesDown()
        {
            List<string> statestoRemove = new List<string>();
            foreach(State state in _states)
            {
                state.TurnsLeft--;

                if(state.TurnsLeft <= 0) statestoRemove.Add(state.Id);
            }

            foreach(string key in statestoRemove)
            {
                _states.Remove(_states.Find(s => s.Id.Equals(key)));
            }
        }

        public int GetValue(EAttribute type)
        {
            return _attributes[(int) type].Value;
        }

        public int GetValue(int type)
        {
            return _attributes[type].Value;
        }

        public int GetMaxValue(EAttribute type)
        {
            return _attributes[(int) type].MaxValue;
        }

        public int GetMaxValue(int type)
        {
            return _attributes[type].MaxValue;
        }

        public void SetResistance(EElement element, int value)
        {
            _resistance[(int) element].BaseValue = value;
        }

        public float GetResistance(int id)
        {
            if(_resistance != null && _resistance.Length > 0)
                return _resistance.ToList().Find(res => (int) res.Id == id).Value;
            
            return 0;
        }

        public float GetResistance(EElement id)
        {
            if(_resistance != null && _resistance.Length > 0)
                return _resistance.ToList().Find(res => res.Id == id).Value;
            
            return 0;
        }

        public void AddState(string stateId)
        {
            State state = _states.ToList().Find(state => state.Id.Equals(stateId));
            BattlerState battlerState = Database.Instance.GetBattlerState(stateId);

            if(state != null)
            {
                state.TurnsLeft = battlerState.turnsToExpire;
            }
            else
            {
                _states.Add(new State(battlerState.name, battlerState.turnsToExpire));
            }
        }

        public void RemoveState(string stateId)
        {
            _states.Remove(_states.Find(s => s.Id.Equals(stateId)));
        }

        public void RemoveAllStates()
        {
            _states.Clear();
        }

        public List<State> GetStates()
        {
            return _states.ToList();
        }

        public State GetState(string id)
        {
            return _states.ToList().Find(state => state.Id.Equals(id));
        }

        public AttributesModel DeepCopy()
        {
            // Create a copy of the attributes
            Attribute[] copiedAttributes = new Attribute[_attributes.Length];
            for (int i = 0; i < _attributes.Length; i++)
            {
                Attribute originalAttr = _attributes[i];
                copiedAttributes[i] = new Attribute
                {
                    Id = originalAttr.Id,
                    BaseValue = originalAttr.BaseValue,
                    BaseMaxValue = originalAttr.BaseMaxValue,
                    VariableAttribute = originalAttr.VariableAttribute,
                    Modifiers = new Dictionary<string, AttributeModifier>(originalAttr.Modifiers),
                };
            }

            // Create a copy of the resistances
            Resistance[] copiedResistances = new Resistance[_resistance.Length];
            for (int i = 0; i < _resistance.Length; i++)
            {
                Resistance originalRes = _resistance[i];
                copiedResistances[i] = new Resistance
                {
                    BaseValue = originalRes.BaseValue,
                    Id = originalRes.Id,
                    Modifiers = new Dictionary<string, ResistanceModifier>(originalRes.Modifiers),
                };
            }

            // Create a new AttributesModel with the copied data
            AttributesModel copiedModel = new AttributesModel(copiedAttributes, copiedResistances, _range);

            return copiedModel;
        }
    }

    [Serializable]
    public class Attribute 
    {
        [SerializeField] [HideLabel] [HorizontalGroup("Attribue")]
        private EAttribute _id;  
        [SerializeField] [VerticalGroup("Attribue/Column2")] [LabelText("Base")] [ProgressBar(0, 999)]
        private int _baseValue = 0;
        [SerializeField] [VerticalGroup("Attribue/Column2")] [LabelText("Min")] [HideInInspector]
        private int _baseMinValue = 0; 
        [SerializeField] [VerticalGroup("Attribue/Column2")] [LabelText("Max")] [ProgressBar(0, 999)]
        private int _baseMaxValue = 0; 
        [SerializeField] [VerticalGroup("Attribue/Column2")] [LabelText("Variable")]
        private bool _variableAttribute = false; 
        private Vector2Int _range;

        public EAttribute Id { get => _id; set => _id = value; }
        public int BaseValue { get => _baseValue; set => _baseValue = value; }
        public int BaseMaxValue { get => _baseMaxValue; set => _baseMaxValue = value; }
        public bool VariableAttribute { get => _variableAttribute; set => _variableAttribute = value; }
        public int BaseMinValue { get => _baseMinValue; set => _baseMinValue = value; }
        public int MaxValue => GetMaxValue();
        public int Value => GetValue();
        public Dictionary<string, AttributeModifier> Modifiers { get; set; }
        public Dictionary<string, AttributeModifier> MaxValueModifiers { get; set; }
        public Vector2Int Range { get => _range; set => _range = value; }

        public Attribute()
        {
            Modifiers = new Dictionary<string, AttributeModifier>();
            MaxValueModifiers = new Dictionary<string, AttributeModifier>();
        }
        
        private int GetValue()
        {
            int modifiersSum = 0; 
            if(!VariableAttribute)
            {
                modifiersSum = Modifiers.Values.Sum(mod => mod.Value); 
            }

            return Mathf.Clamp(BaseValue + modifiersSum, _range.x, _range.y);
        }

        private int GetMaxValue()
        {
            int modifiersSum = 0; 
            if(VariableAttribute)
            {
                Modifiers.Values.ForEach(mod => { modifiersSum += mod.Value; });
            }

            return Mathf.Clamp(BaseMaxValue + modifiersSum, _range.x, _range.y);
        }

        public override string ToString()
        {
            var modifiers = string.Join(", ", Modifiers.Select(kv => $"{kv.Key}: {kv.Value}"));

            return $"Attribute [Id: {Id}, BaseValue: {BaseValue}, BaseMaxValue: {BaseMaxValue}, MaxValue: {MaxValue}, Value: {Value}, " +
                $"VariableAttribute: {VariableAttribute}, Modifiers: {{{modifiers}}}]";
        }
    }

    public class AttributeModifier
    {
        public int Value { get; set; }
        public bool IsTimeLimited { get; set; }
        public int TurnCount { get; set; }
    }

    [Serializable]
    public class Resistance 
    {
        [SerializeField] [ReadOnly] [HideLabel] [HorizontalGroup("Resistance")]
        private EElement _id;
        [SerializeField] [HideLabel] [ProgressBar(-100, 100)] [HorizontalGroup("Resistance")]
        private float _baseValue;

        public Dictionary<string, ResistanceModifier> Modifiers { get; set; }
        public EElement Id { get => _id; set => _id = value; }
        // public int ElementId { get => _elementId; set => _elementId = value; }
        public float BaseValue { get => _baseValue; set => _baseValue = value; }
        public float Value => GetValue();

        public Resistance()
        {
            Modifiers = new Dictionary<string, ResistanceModifier>();
        }

        private float GetValue()
        {
            float modifiersSum = Modifiers.Values.Sum(mod => mod.Value); 
            
            return BaseValue + modifiersSum;
        }
    }

    public class ResistanceModifier
    {
        public int Value { get; set; }
        public bool IsTimeLimited { get; set; }
        public int TurnCount { get; set; }
    }

    [Serializable]
    public class State 
    {
        private string _id;
        private int _turnsleft;
        private bool _executedOnce;

        public string Id { get => _id; set => _id = value; }
        public int TurnsLeft { get => _turnsleft; set => _turnsleft = value; }
        public bool ExecutedOnce { get => _executedOnce; set => _executedOnce = value; }

        public State(string id, int turnsLeft)
        {
            _id = id;
            _turnsleft = turnsLeft;
        }
    }

}
