using AssetKits.ParticleImage;
using Atlas.Effects;
using Atlas.Pooling;
using UnityEngine;

public class ParticleImageEffect : MonoBehaviour, IEffect
{
    [SerializeField] 
    private float _time;

    [SerializeField] 
    private ParticleImage _particleImage;
    private float _currentTime;

    #region IPoolable Members
    public string Name => gameObject.name;

    public GameObject Prefab => gameObject;

    public int AmountToPool => 3;

    public bool ShouldExpand => true;

    public bool LazyPool => false;

    public string EffectName => gameObject.name;
    #endregion

    #region IEffect Members
    public Transform Follow { get; set; }
    public bool IsFollowing { get; set; }

    public float EffectTime => _time;
    #endregion

    public void OnObjectSpawned()
    {
        
    }

    public void OnPreObjectSpawned()
    {
        throw new System.NotImplementedException();
    }

    public void Play()
    {
        _particleImage.Play();
    }

    private void OnEnable() 
    {
        _currentTime = 0;
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
                // gameObject.transform.SetParent()
                gameObject.SetActive(false);
                IsFollowing = false;
            }
        }    
    }
}
