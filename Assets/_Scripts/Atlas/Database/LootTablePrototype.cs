using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Atlas/Data/LootTable")]
    public class LootTablePrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public LootTable lootTable;
    }
}