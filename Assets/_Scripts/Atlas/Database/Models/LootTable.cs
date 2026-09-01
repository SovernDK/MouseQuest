using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class LootTable
    {
        public string name;
        public List<Loot> loots;
    }

    [Serializable]
    public class Loot
    {
        public ItemPrototype prototype;
        public int amount;
        [Range(0, 1)]
        public float weight;
    }
}
