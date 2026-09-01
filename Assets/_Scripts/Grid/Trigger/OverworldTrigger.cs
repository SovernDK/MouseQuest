using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Atlas.Utility;
using System;
using Atlas.Player;

[Serializable]
[RequireComponent(typeof(Collider))]
public abstract class OverworldTrigger : MonoBehaviour, IOverworldComponent
{
    [SerializeField]
    protected Image _icon;
    [SerializeField]
    [ES3Serializable]
    protected bool touchTrigger;
    [SerializeField]
    [ES3Serializable]
    protected bool _triggerable = true;

    [Inject] [ES3NonSerializable]
    protected PlayerSystem _player;
    [Inject] [ES3NonSerializable]
    protected Config _config;
    
    protected Tweener _iconAnim;
    protected Vector3 _originalPosition;
    protected string _serializationKey;

    public bool TouchTrigger { get => touchTrigger; set => touchTrigger = value; }
    public string SerializationKey { get => _serializationKey; set => _serializationKey = value; }

    private void Start()
    {
        _originalPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!_triggerable) return;

        _player.TriggerInRange = this;
    }

    private void OnTriggerExit(Collider other) 
    {
        if(!_triggerable) return;

        _player.TriggerInRange = null;
    }

    public virtual void Trigger() 
    { 
        if(!_triggerable) return;

        _player.TriggerInRange = null;
    }
}

