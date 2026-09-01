using Atlas.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.Effects 
{
    public class SoundEffectSource : MonoBehaviour, IEffect
    {
        [SerializeField] 
        private AudioSource _source;
        [SerializeField] 
        private AudioClip _clip;

        [SerializeField] 
        private float _time;

        [SerializeField] 
        private bool randomizePitch;
        
        [SerializeField] [ShowIf("@randomizePitch == true")]
        private Vector2 pitchRange;
        private float _currentTime;
        public string EffectName { get => Name; }

        #region IPoolable Members
        public string Name => gameObject.name;

        public GameObject Prefab => gameObject;

        public int AmountToPool => 2;

        public bool ShouldExpand => true;

        public bool LazyPool => true;

        public Transform Follow { get; set; }

        public bool IsFollowing { get; set; }
        #endregion
        public AudioClip Clip { get => _clip; set => _clip = value; }
        public bool RandomizePitch { get => randomizePitch; set => randomizePitch = value; }
        public Vector2 PitchRange { get => pitchRange; set => pitchRange = value; }

        public float EffectTime => _time;

        public void OnObjectDisbaled()
        {
        }

        public void OnObjectEnabled()
        {
            
        }

        public void OnSpawned()
        {
        }

        private void OnEnable() 
        {
            if(randomizePitch)
                _source.pitch = Random.Range(pitchRange.x, pitchRange.y); 
        }

        public void Play()
        {
            _source.clip = Clip;
            _source.Play(0);   
            _currentTime = 0; 
            _time = _source.clip.length;
        }

        private void Update() 
        {
            if(gameObject.activeSelf)
            {
                _currentTime += Time.deltaTime;
                
                if(IsFollowing && Follow != null)
                {
                    transform.position = Follow.transform.position;
                }

                if(_currentTime > _time)
                {
                    transform.position = Vector3.zero;
                    gameObject.SetActive(false);
                }
            }    
        }

        public void OnObjectSpawned()
        {
            
        }

        public void OnPreObjectSpawned()
        {
            throw new System.NotImplementedException();
        }
    }
}