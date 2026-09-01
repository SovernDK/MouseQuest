using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;
using Atlas.Utility;

namespace Atlas.Pooling 
{
    public class PoolSystem : Singleton<PoolSystem>
    {
        [System.Serializable]
        public class Pool
        {
            [HideInInspector] public string poolName;
            [HideLabel] public GameObject objectToPool;
            [HorizontalGroup("Amount", Gap = 10)] [LabelText("Amount")] public int amountToPool;
            [HorizontalGroup("Amount")] [LabelText("Expand?")] public bool shouldExpand = true;
            [HorizontalGroup("Amount")] [LabelText("Lazy?")] public bool lazyLoading = true;
            [HideInInspector] public List<GameObject> pooledObjects;
        }

        [ListDrawerSettings(NumberOfItemsPerPage = 10, ShowItemCount = true)]  [Searchable]
        public List<Pool> pools;

        [Inject] 
        private DiContainer _container;

        protected override void Awake()
        {
            base.Awake();

            Load();
            InitializePools();
        }

        void InitializePools()
        {
            foreach (Pool pool in pools)
            {
                pool.poolName = pool.objectToPool.name;
                pool.pooledObjects = new List<GameObject>();
                if(pool.lazyLoading)
                {
                    for (int i = 0; i < pool.amountToPool; i++)
                    {
                        GameObject obj = Instantiate(pool.objectToPool, transform);
                        obj.SetActive(false);
                        pool.pooledObjects.Add(obj);
                    }
                }
            }
        }

        void Load()
        {
            pools.Clear();
            foreach (GameObject prefab in Resources.LoadAll<GameObject>("Prefabs"))
            {
                if (prefab.TryGetComponent(out IPoolable poolableComponent))
                {
                    Pool newPool = new Pool
                    {
                        poolName = poolableComponent.Name,
                        objectToPool = poolableComponent.Prefab,
                        amountToPool = poolableComponent.AmountToPool,
                        shouldExpand = poolableComponent.ShouldExpand,
                        lazyLoading = poolableComponent.LazyPool
                    };
                    pools.Add(newPool);
                }
            }
        }

        public GameObject SpawnFromPool(string poolName, Vector3 position = default, Quaternion rotation = default)
        {
            Pool pool = pools.Find(p => p.poolName == poolName);
            if (pool == null)
            {
                Debug.LogWarning($"Pool with name {poolName} doesn't exist.");
                return null;
            }

            foreach (var obj in pool.pooledObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.transform.parent = transform;
                    obj.GetComponent<IPoolable>().OnPreObjectSpawned();
                    obj.SetActive(true);
                    
                    _container.InjectGameObject(obj);

                    obj.GetComponent<IPoolable>().OnObjectSpawned();
                    return obj;
                }
            }

            if (pool.pooledObjects.Count < pool.amountToPool || pool.shouldExpand)
            {
                GameObject obj = _container.InstantiatePrefab(pool.objectToPool, position, rotation, transform);

                pool.pooledObjects.Add(obj);

                obj.GetComponent<IPoolable>().OnObjectSpawned();
                return obj;
            }

            return null; // No object available and pool cannot expand.
        }

        public void ReturnAll()
        {
            foreach (Pool pool in pools)
            {
                foreach(GameObject pooledObject in pool.pooledObjects)
                {
                    ReturnToPool(pooledObject);
                }
            }
        }

        public void ReturnAllByPool(string byPool)
        {
            foreach (Pool pool in pools)
            {
                if(pool.poolName.Equals(byPool))
                {
                    foreach(GameObject pooledObject in pool.pooledObjects)
                    {
                        ReturnToPool(pooledObject);
                    }
                }
            }
        }

        // Method to return an object to the pool (simply deactivates it).
        public void ReturnToPool(GameObject obj)
        {
            obj.GetComponent<IPoolable>().OnReturnedToPool();
            obj.SetActive(false);
        }

        public void Validate()
        {
            // throw new System.NotImplementedException();
        }
        
        private void OnDestroy()
        {
            ReturnAll();
            pools.Clear();
        }
    }
}