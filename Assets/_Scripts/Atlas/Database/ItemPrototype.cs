using UnityEngine;
using Sirenix.OdinInspector;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Item", menuName = "Atlas/Data/Item")]
    public class ItemPrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Item item;
    }
}