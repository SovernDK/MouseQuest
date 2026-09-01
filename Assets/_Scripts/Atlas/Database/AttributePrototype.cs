using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [CreateAssetMenu(fileName = "Attribute", menuName = "Atlas/Data/Attribute")]
    public class AttributePrototype : ScriptableObject
    {
        [HideLabel] 
        public Attribute attribute;
    }
}