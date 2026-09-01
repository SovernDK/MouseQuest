using Atlas.DB;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    public class BattlerStatePrototype : ScriptableObject
    {
        [InlineProperty] [HideLabel]
        public BattlerState state;
    }
}