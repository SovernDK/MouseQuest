using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Atlas/Data/Progression")]
    public class ProgressionPrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Progression data;
    }
}