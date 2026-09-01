using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    public class EnemyPrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Enemy data;
    }
}