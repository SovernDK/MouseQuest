using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Atlas.Enums;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atlas.DB
{
    [GlobalConfig("Assets/Resources/Data/")]
    public class Database : GlobalConfig<Database>
    {
        [ReadOnly]
        public List<PlayerCharacterPrototype> AllCharacters;
        [ReadOnly]
        public List<EnemyPrototype> AllEnemies;
        [ReadOnly]
        public List<SpellPrototype> AllSpells;
        [ReadOnly]
        public List<AttributePrototype> AllAttributes;
        [ReadOnly]
        public List<ResistancePrototype> AllResistance;
        [ReadOnly]
        public List<ItemPrototype> AllItems;
        [ReadOnly]
        public List<BattlerStatePrototype> AllBattlerStates;
        [ReadOnly]
        public List<ShopPrototype> AllShops;
        public List<Battle> AllBattles;
    
        #if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void LoadAll()
        {
            AllCharacters = Load<PlayerCharacterPrototype>();
            AllEnemies = Load<EnemyPrototype>();
            AllSpells = Load<SpellPrototype>();
            AllItems = Load<ItemPrototype>();
            AllAttributes = Load<AttributePrototype>();
            AllResistance = Load<ResistancePrototype>();
            AllBattlerStates = Load<BattlerStatePrototype>();
            AllShops = Load<ShopPrototype>();
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public List<T> Load<T>() where T : Object
        {
            List<T> result = new List<T>();

            // Use typeof(T).Name to find assets of type T
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T prototype = AssetDatabase.LoadAssetAtPath<T>(path);
                if (prototype != null)
                {
                    result.Add(prototype);
                }
            }

            return result;
        }
        #endif

        public PlayerCharacter GetCharacter(int id)
        {
            return AllCharacters[id].playerCharacter;
        }

        public Enemy GetEnemy(int id)
        {
            return AllEnemies[id].data;
        }

        public Enemy GetEnemy(string id)
        {
            return AllEnemies.Find(prototype => prototype.data.name.Equals(id)).data;
        }

        public List<Enemy> GetEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();
            AllEnemies.ForEach(prototype => enemies.Add(prototype.data));
            return enemies;
        }

        public Item GetItem(string id)
        {
            return AllItems.Find(prototype => prototype.item.name.Equals(id)).item;
        }

        public Spell GetSpell(string id)
        {
            ConsoleProDebug.LogToFilter($"spell ID: {id}", "SpellSystem");
            return AllSpells.Find(prototype => prototype.spell.name.Equals(id)).spell;
        }

        public Battle GetBattle(int id)
        {
            return AllBattles[id];
        }

        public Attribute GetAttribute(int id)
        {
            return AllAttributes.Find(prototype => (int) prototype.attribute.id == id).attribute;
        }

        public Attribute GetAttribute(string id)
        {
            return AllAttributes.Find(prototype => prototype.name.Equals(id)).attribute;
        }

        public List<Attribute> GetAttributes()
        {
            List<Attribute> attributes = new List<Attribute>();
            AllAttributes.ForEach(prototype => attributes.Add(prototype.attribute));
            return attributes;
        }

        public Resistance GetResistance(int id)
        {
            return AllResistance.Find(prototype => prototype.data.id == (EElement) id).data;
        }

        public List<Resistance> GetResistances()
        {
            List<Resistance> resistances = new List<Resistance>();
            AllResistance.ForEach(prototype => resistances.Add(prototype.data));
            return resistances;
        }

        public List<Item> GetItems()
        {
            List<Item> itemList = new List<Item>();
            AllItems.ForEach(prototype => itemList.Add(prototype.item));
            return itemList;
        }

        public List<Spell> GetAllSpells()
        {
            List<Spell> spellList = new List<Spell>();
            AllSpells.ForEach(prototype => spellList.Add(prototype.spell));
            return spellList;
        }

        public List<BattlerState> GetBattlerStates()
        {
            List<BattlerState> battlerStateList = new List<BattlerState>();
            AllBattlerStates.ForEach(prototype => battlerStateList.Add(prototype.state));
            return battlerStateList;
        }

        public BattlerState GetBattlerState(string id)
        {
            return AllBattlerStates.Find(prototype => prototype.state.name.Equals(id)).state;
        }

        public Shop GetShop(string id)
        {
            return AllShops.Find(prototype => prototype.data.name.Equals(id)).data;
        }

        public List<Shop> GetShops()
        {
            List<Shop> shopList = new List<Shop>();
            AllShops.ForEach(prototype => shopList.Add(prototype.data));
            return shopList;
        }
    }
}