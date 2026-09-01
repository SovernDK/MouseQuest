using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Shop", menuName = "Atlas/Data/Shop")]
    public class ShopPrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Shop data;
    }
}