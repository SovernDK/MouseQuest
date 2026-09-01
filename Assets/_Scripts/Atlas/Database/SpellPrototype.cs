using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Atlas/Data/Spell")]
    public class SpellPrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public Spell spell;
    }
}