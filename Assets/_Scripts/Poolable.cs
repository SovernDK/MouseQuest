using UnityEngine;
using UnityEngine.Events;

namespace Atlas.Pooling
{
    public class Poolable : MonoBehaviour, IPoolable
    {
        [SerializeField]
        public bool _lazyPool;
        [SerializeField]
        public bool _shouldExpand;
        [SerializeField]
        public int _amountToPool;

        public UnityEvent ObjectSpawned;
        public UnityEvent PreObjectSpawned;
        public UnityEvent ReturnedToPool;

        public string Name => gameObject.name;
        public GameObject Prefab => gameObject;
        public int AmountToPool => _amountToPool;
        public bool ShouldExpand => _shouldExpand;
        public bool LazyPool => _lazyPool;

        public void OnObjectSpawned()
        {
            ObjectSpawned.Invoke();
        }

        public void OnPreObjectSpawned()
        {
            PreObjectSpawned.Invoke();
        }

        public void OnReturnedToPool()
        {
            ReturnedToPool.Invoke();
        }
    }
}
