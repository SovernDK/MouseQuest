using System.Collections;
using System.Collections.Generic;
using Atlas.AI.Grid;
using Atlas.DB;
using Atlas.Effects;
using CMF;
using UnityEngine;
using Zenject;
using Atlas.Core.Serialization;
using System;
using CharacterSheet;
using Atlas.Utility;
using UnityEngine.Events;

namespace Atlas.Player
{
    [Serializable]
    public class PlayerSystem : MonoBehaviour, ISerializableObject
    {
        [SerializeField]
        private ItemAddedRow _row;

        private PlayerCharacter _playerCharacter;

        private PlayerFSM _playerFSM;
        private InventorySystem _inventorySystem;
        private AttributeSystem _attributeSystem;
        private SpellSystem _spellSystem;
        private PlayerBattler _playerBattler;
        private DialogueSystem _dialogueSystem;
        
        private OverworldTrigger _triggerInRange;
        private Queue<(int, Item)> _items;

        private Animator _animator;

        public SimpleWalkerController Controller { get; set; }
        public PlayerFSM PlayerFSM { get => _playerFSM; set => _playerFSM = value; }
        public InventorySystem InventorySystem { get => _inventorySystem; set => _inventorySystem = value; }
        public AttributeSystem AttributeSystem { get => _attributeSystem; set => _attributeSystem = value; }
        public PlayerBattler Battler { get => _playerBattler; set => _playerBattler = value; }
        public Queue<BattleCommand> Commands { get; set; }
        public Queue<PlayerCommand> PlayerCommands { get; set; }
        public DialogueSystem DialogueSystem { get => _dialogueSystem; set => _dialogueSystem = value; }
        public SpellSystem SpellSystem { get => _spellSystem; set => _spellSystem = value; }
        public InputReceiver InputReceiver { get; set; }
        public OverworldTrigger TriggerInRange { get => _triggerInRange; set => _triggerInRange = value; }
        public string _serializationKey = "Player";
        public string SerializationKey { get => _serializationKey; set => _serializationKey = value; }
        public PlayerCharacter PlayerCharacter { get => _playerCharacter; set => _playerCharacter = value; }

        public UnityEvent<PlayerSystem> Initialized;

        [Serializable]
        private class PlayerSerializationData
        {
            public InventoryModel InventoryModel;
            public EquipmentModel EquipmentModel;
            public AttributesModel AttributesModel;
        } 

        private void Awake() 
        {
            _playerCharacter = Database.Instance.GetCharacter(ES3.Load<int>("character"));
        }

        private void Start() 
        {
            Initialize();
            Initialized.Invoke(this);
        }

        public void Initialize() 
        {
            _items = new Queue<(int, Item)>();
            _animator = GetComponent<Animator>();

            if(TryGetComponent(out SimpleWalkerController walker)) 
                Controller = walker;
        }

        private void Update() 
        {
            if(Controller != null)
            {
                _animator.SetFloat("velocity", Controller.GetVelocity().magnitude);

                if(Controller.GetMovementVelocity().x > 0)
                {
                    GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if(Controller.GetMovementVelocity().x < 0)
                {
                    GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    
                }
            }
        }

        public void Teleport(Vector3 coordinates)
        {
            ConsoleProDebug.LogToFilter($"Teleported to world pos.: {coordinates + Vector3.up}", "Player");
            GetComponent<Rigidbody>().MovePosition(coordinates + Vector3.up);
        }

        public void UpdateUI() 
        {
            // _attributeSystem.Refresh();
            // _inventorySystem.Refresh();
        }

        public IEnumerator FSMUpdate()
        {
            while(true)
            {
                yield return StartCoroutine(_playerFSM.Update());
            }
        }

        public string GetObjectKey()
        {
            return GetInstanceID().ToString();
        }

        public void Save(ES3Settings settings, string path = "")
        {
            ES3.Save($"{SerializationKey}_Transform", transform, settings);
            ES3.Save($"{SerializationKey}_Inventory", InventorySystem.InventoryModel, settings);
            ES3.Save($"{SerializationKey}_Equipment", InventorySystem.EquipmentModel, settings);
            ES3.Save($"{SerializationKey}_Attributes", AttributeSystem.Attributes, settings);
        }

        public void Load(ES3Settings settings, string path = "")
        {
            Transform playerTransform = ES3.Load<Transform>($"{SerializationKey}_Transform");
            GetComponent<Rigidbody>().MovePosition(playerTransform.position);
            GetComponent<Rigidbody>().MoveRotation(playerTransform.rotation);

            ES3.LoadInto($"{SerializationKey}_Attributes", _attributeSystem.Attributes);
            ES3.LoadInto($"{SerializationKey}_Inventory", _inventorySystem.InventoryModel);
            ES3.LoadInto($"{SerializationKey}_Equipment", _inventorySystem.EquipmentModel);
        }

        public SerializedObject GetSerializedObject()
        {
            return new SerializedObject()
            {
                name = Util.RemoveNumberFromDuplicatedName(gameObject.name),
                key = SerializationKey,
            };
        }

        public void Initialize(string key, ES3Settings settings)
        {
            _serializationKey = key;
            // _settings = settings;
        }
    }

    public enum EConsumableEffect
    {
        IncreaseAttribute, DecreaseAttribute, MaxAttributeIncrease, MaxAttributeDecrease
    }
}