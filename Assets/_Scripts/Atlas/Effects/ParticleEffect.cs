using Atlas.Pooling;
using UnityEngine;

namespace Atlas.Effects
{
    public class ParticleEffect : MonoBehaviour, IEffect
    {
        [SerializeField] 
        private float effectTime;
        
        [SerializeField] 
        private ParticleSystem _particleSystem;
        private float _time;
        
        #region IPoolable Members
        public string Name => gameObject.name;

        public GameObject Prefab => gameObject;

        public int AmountToPool => 2;

        public bool ShouldExpand => true;

        public bool LazyPool => false;
        #endregion

        public string EffectName => gameObject.name;

        public Transform Follow { get; set; }

        public bool IsFollowing { get; set; }

        public float EffectTime => effectTime;

        public void Play()
        {
            foreach(Transform particle in transform)
            {
                if(TryGetComponent(out ParticleSystem system))
                {
                    system.Play();
                }
            }
            // _particleSystem.Play();
        }

        private void OnEnable() 
        {
            _time = 0;
        }

        private void Update() 
        {
            if(gameObject.activeSelf)
            {
                _time += Time.deltaTime;

                if(IsFollowing && Follow != null)
                {
                    transform.position = Follow.transform.position;
                }

                if(_time > effectTime)
                {
                    transform.position = Vector3.zero;
                    gameObject.SetActive(false);
                    IsFollowing = false;
                }
                
            }    
        }
    }
}