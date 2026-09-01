using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Battle", menuName = "Atlas/Data/Battle")]
    public class BattlePrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Battle battle;
    }
}