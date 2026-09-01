using System.Collections.Generic;
using Atlas.Enums;
using UnityEngine;

namespace Atlas.DB
{
    public class DatabaseSystem : MonoBehaviour
    {
        public Database db;

        public Enemy GetEnemy(int id)
        {
            return Database.Instance.AllEnemies[id].data;
        }

        public Item GetItem(int id)
        {
            return Database.Instance.AllItems[id].item;
        }

        public Spell GetSpell(int id)
        {
            return Database.Instance.AllSpells[id].spell;
        }

        public Battle GetBattle(int id)
        {
            return Database.Instance.AllBattles[id];
        }

        public Attribute GetAttribute(int id)
        {
            return Database.Instance.AllAttributes.Find(x => (int) x.attribute.id == id).attribute;
        }

        public Attribute GetAttribute(string id)
        {
            return Database.Instance.AllAttributes.Find(x => x.name.Equals(id)).attribute;
        }

        public List<Attribute> GetAttributes()
        {
            List<Attribute> attributes = new List<Attribute>();
            Database.Instance.AllAttributes.ForEach(prototype => attributes.Add(prototype.attribute));
            return attributes;
        }

        public List<Item> GetItems()
        {
            List<Item> itemList = new List<Item>();
            Database.Instance.AllItems.ForEach(prototype => itemList.Add(prototype.item));
            return itemList;
        }

        public List<Spell> GetSpells()
        {
            List<Spell> spellList = new List<Spell>();
            Database.Instance.AllSpells.ForEach(prototype => spellList.Add(prototype.spell));
            return spellList;
        }
    }
}