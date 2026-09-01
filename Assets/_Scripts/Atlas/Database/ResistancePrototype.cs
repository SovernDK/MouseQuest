using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Resistance", menuName = "Atlas/Data/Resistance")]
    public class ResistancePrototype : ScriptableObject
    {
        [HideLabel] 
        public Resistance data;
    }
}