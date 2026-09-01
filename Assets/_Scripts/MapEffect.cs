using Atlas.Pooling;
using Atlas.Utility;
using Atlas.Core.Serialization;
using UnityEngine;

namespace Atlas.MapEditor 
{
    public class MapEffect : MonoBehaviour
    {
        #region IPoolable
        public string Name => gameObject.name;
        public GameObject Prefab => gameObject;
        public int AmountToPool => 1;
        public bool ShouldExpand => false;
        public bool LazyPool => false;
        #endregion

        public string GetObjectId()
        {
            return GetInstanceID().ToString();
        }

        public void Load(MapSerializedObject saveData)
        {
            
        }

        public void OnObjectSpawned()
        {
            
        }

        public void OnPreObjectSpawned()
        {
            
        }

        public MapSerializedObject Save(string id)
        {
            return new MapSerializedObject()
            {
                name = Util.RemoveNumberFromDuplicatedName(gameObject.name),
                objectId = id,
                // type = "MapEffect",
                // position = transform.position,
                // rotation = transform.rotation,
                // data = ""
            };
        }
    }
}