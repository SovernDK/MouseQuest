using UnityEngine;

namespace Atlas.Pooling 
{
    public interface IPoolable
    {
        #region IPoolable Properties
        string Name { get; }
        GameObject Prefab { get; }
        int AmountToPool { get; }
        bool ShouldExpand { get; }
        bool LazyPool { get; }
        #endregion

        public void OnObjectSpawned();
        public void OnPreObjectSpawned();
        public void OnReturnedToPool();
    }
}