using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atlas.Core.Serialization;
using Atlas.Map;
using Atlas.Player;
using Atlas.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class SaveLoadSystem : MonoBehaviour
{
    private SerializationSystem _serializationSystem;

    [Inject(Optional = true)]
    private PoolSystem _pool;
    [Inject(Optional = true)]
    private PlayerSystem _player;
    [Inject(Optional = true)]
    private MapSystem _mapSystem;

    private ES3Settings _settings;

    private void Start() 
    {
        _serializationSystem = GetComponent<SerializationSystem>();
        _settings = new ES3Settings(ES3.Location.Cache);
    }

    [Button("Save")]
    public void QuickSave()
    {
        _mapSystem.Save(_settings);
        IEnumerable<ISerializableObject> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, 
                                                                                FindObjectsSortMode.InstanceID)
                                                        .OfType<ISerializableObject>();

        foreach (ISerializableObject saveable in saveables)
        {
            saveable.Save(_settings);
        }

        ES3.StoreCachedFile();
    }

    [Button("Load")]
    public void QuickLoad()
    {
        _mapSystem.LoadFloor(ES3.Load<string>("CurrentFloor"));
        _mapSystem.LoadMap(ES3.Load<string>("CurrentMap"));

        // IEnumerable<ISerializableObject> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, 
        //                                                                         FindObjectsSortMode.InstanceID)
        //                                                 .OfType<ISerializableObject>();

        // foreach (ISerializableObject saveable in saveables)
        // {
        //     saveable.Load(_settings);
        // }
    }

    public void LoadFloor()
    {

    }

    public void LoadMap()
    {
        
    }
}
