using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Atlas.Pooling;
using Atlas.Core;
using Zenject;
using Atlas.Effects;
using Atlas.Utility;

namespace Atlas.Effects 
{
    [Serializable]
    public class EffectsSystem : MonoBehaviour
    {
        private Queue<Effect> effectQueue = new Queue<Effect>();
        private Effect _currentEffect;
        private GameObject _currentEffectObject;

        [Inject]
        public ResourcesSystem ResourcesSystem { get; }

        private void Awake() 
        {
            Debug.Log($"Start ProcessEffectQueue");
            StartCoroutine(ProcessEffectQueue());
        }

        private IEnumerator ProcessEffectQueue()
        {
            while (true)
            {
                if (effectQueue.Count > 0)
                {
                    _currentEffect = effectQueue.Dequeue();

                    yield return _currentEffect.Play(this, PoolSystem.Instance);
                }
                else
                {
                    yield return null;
                }
            }
        }

        public void AddEffect(Effect effect)
        {
            effectQueue.Enqueue(effect);
        }

        public void StopCurrentEffect()
        {
            if(_currentEffectObject != null)
            {
                PoolSystem.Instance.ReturnToPool(_currentEffectObject);
            }
        }

        public void StopAllEffects()
        {
            PoolSystem.Instance.ReturnAll();
        }
    }
}

[Serializable]
public class Effect
{
    protected string _effectName;
    protected Vector3 _position;
    protected Quaternion _rotation = Quaternion.identity;
    protected float _delay = 0;
    protected float _time = 0;
    protected Transform _follow = null;
    protected Transform _parent = null;

    public float Delay { get => _delay; set => _delay = value; }
    public Transform Follow { get => _follow; set => _follow = value; }
    public Transform Parent { get => _parent; set => _parent = value; }
    public Quaternion Rotation { get => _rotation; set => _rotation = value; }
    public Vector3 Position { get => _position; set => _position = value; }
    public float Time { get => _time; }

    public Effect(string name, Vector3 pos)
    {
        _effectName = name;
        _position = pos;
    }

    public Effect SetRotation(Quaternion rotation)
    {
        _rotation = rotation;
        return this;
    }

    public Effect SetDelay(float delay)
    {
        _delay = delay;
        return this;
    }

    public Effect SetFollow(Transform follow)
    {
        _follow = follow;
        return this;
    }

    public Effect SetParent(Transform parent)
    {
        _parent = parent;
        return this;
    }

    public Effect SetTime(float time)
    {
        _time = time;
        return this;
    }

    public Effect Build()
    {
        return this;
    }

    public virtual IEnumerator Play(EffectsSystem effectsSystem, PoolSystem poolSystem)
    {
        yield return new WaitForSeconds(_delay);
    }
}

[Serializable]
public class ParticleEffect : Effect
{
    public ParticleEffect(string name, Vector3 pos) : base(name, pos) {}

    public override IEnumerator Play(EffectsSystem effectsSystem, PoolSystem poolSystem)
    {
        yield return base.Play(effectsSystem, poolSystem);

        ConsoleProDebug.LogToFilter($"Spawn effect {_effectName}", "EffectsSystem");

        GameObject effect = poolSystem.SpawnFromPool(_effectName, _position, _rotation);
        // _time = effect.GetComponent<IEffect>().EffectTime;

        if(effect == null)
        {
            ConsoleProDebug.LogAsType($"Particle effect {_effectName} doesnt exist", "Error");
            yield break;
        } 

        if(_parent != null) effect.transform.SetParent(_parent);
        if(_follow != null) 
        {
            effect.GetComponent<IEffect>().Follow = _follow;
            effect.GetComponent<IEffect>().IsFollowing = true;
        }
        effect.GetComponent<IEffect>().Play();
    }
}

[Serializable]
public class SoundEffect : Effect
{
    public string clipName;
    public bool randomizePitch;

    public SoundEffect(string name, Vector3 pos) : base(name, pos) 
    {
        _effectName = "Sfx";
        clipName = name;
    }

    public override IEnumerator Play(EffectsSystem effectsSystem, PoolSystem poolSystem)
    {
        yield return base.Play(effectsSystem, poolSystem);

        ConsoleProDebug.LogToFilter($"Spawn effect {_effectName}", "EffectsSystem");

        GameObject effect = poolSystem.SpawnFromPool(_effectName, _position, _rotation);
        // _time = effect.GetComponent<IEffect>().EffectTime;

        if(_parent != null) effect.transform.SetParent(_parent);
        if(_follow != null) 
        {
            effect.GetComponent<IEffect>().Follow = _follow;
            effect.GetComponent<IEffect>().IsFollowing = true;
        }

        AudioClip clip = effectsSystem.ResourcesSystem.LoadClip(clipName);

        effect.GetComponent<SoundEffectSource>().Clip = clip;
        effect.GetComponent<SoundEffectSource>().RandomizePitch = randomizePitch;
        effect.GetComponent<SoundEffectSource>().PitchRange = new Vector2(1, 2);
        
        effect.GetComponent<IEffect>().Play();
    }
}

[Serializable]
public class DamageNumberEffect : Effect
{
    float _value = 0;

    public DamageNumberEffect(string name, Vector3 pos) : base(name, pos) 
    {
        _effectName = "DamageNumber";
    }

    public Effect SetDamageValue(float value)
    {
        _value = value;
        return this;
    }

    public override IEnumerator Play(EffectsSystem effectsSystem, PoolSystem poolSystem)
    {
        yield return base.Play(effectsSystem, poolSystem);

        ConsoleProDebug.LogToFilter($"Spawn effect {_effectName}", "EffectsSystem");
        
        Config.Instance.damageNumbers.Spawn(_position, _value);
    }
}