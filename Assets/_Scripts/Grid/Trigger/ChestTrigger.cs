using System;
using System.Collections.Generic;
using Atlas.DB;
using Atlas.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class ChestTrigger : OverworldTrigger
{
    [ES3Serializable]
    [SerializeField] [FoldoutGroup("Items")] 
    private List<ItemPrototype> _itemsGained;
    [SerializeField] [ES3NonSerializable]    
    private GameObject openedFx;
    [SerializeField] [ES3NonSerializable]    
    private Transform openFxPos;
    [SerializeField] [ES3NonSerializable]    
    private GameObject _modelOpened;
    [SerializeField] [ES3NonSerializable]    
    private GameObject _modelClosed;

    [ES3Serializable]
    private bool _opened;

    public void OnObjectSpawned()
    {
        _originalPosition = Vector3.zero;
    }

    public override void Trigger()
    {
        base.Trigger();

        if(!_opened)
        {
            _opened = true;
            PoolSystem.Instance.SpawnFromPool(openedFx.name, openFxPos.position);
            _modelOpened.SetActive(true);
            _modelClosed.SetActive(false);

            // foreach (ItemPrototype item in _itemsGained)
            // {
            //     _player.InventorySystem.AddItem(item.item.id);
            // }
        }
    }
}
