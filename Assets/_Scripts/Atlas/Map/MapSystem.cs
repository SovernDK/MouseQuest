using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Atlas.Core.Serialization;
using Atlas.Pooling;
using Atlas.Core;
using System.ComponentModel;
using Atlas.Effects;
using UnityEngine.Rendering;
using Atlas.AI.Grid;
using Atlas.Utility;
using TMPro;
using Unity.AI.Navigation;
using Atlas.Player;


#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine.Events;

namespace Atlas.Map
{
    public class MapSystem : MonoBehaviour
    {
        [SerializeField]
        private Light _mainLight;
        [SerializeField] [ReadOnly(true)]
        private GameObject _current;
        [SerializeField] [ReadOnly(true)]
        private List<GameObject> _currentMaps;
        [SerializeField] [ReadOnly(true)]
        private List<GameObject> _currentMapObjects;
        [SerializeField]
        private Transform _floorParent;
        [SerializeField]
        private NavMeshSurface _navMeshSurface;
        [SerializeField]
        private bool _debug;

        private string _currentFloor;

        GridSystem _gridSystem;
        PlayerSystem _player;
        EffectsSystem _effectsSystem;
        ResourcesSystem _resourcesSystem;
        Volume _currentVolume;
        // CameraSystem _cameraSystem;
        TransitionsSystem _transitionsSystem;
        
        // [Inject]
        // public GridSystem GridSystem { get => _gridSystem; set => _gridSystem = value; }
        [Inject]
        public PlayerSystem Player { get => _player; set => _player = value; }
        [Inject]
        public EffectsSystem EffectsSystem { get => _effectsSystem; set => _effectsSystem = value; }
        [Inject]
        public ResourcesSystem ResourcesSystem { get => _resourcesSystem; set => _resourcesSystem = value; }
        [Inject]
        public Volume CurrentVolume { get => _currentVolume; set => _currentVolume = value; }
        // [Inject]
        // public CameraSystem CameraSystem { get => _cameraSystem; set => _cameraSystem = value; }
        [Inject]
        public TransitionsSystem TransitionsSystem { get => _transitionsSystem; set => _transitionsSystem = value; }

        MapSerializeSystem _mapsSerializeSystem;
        
        public UnityEvent OnMapLoaded { get; private set; }
        public UnityEvent OnMapStartedLoading { get; private set; }

        private void Awake() 
        {
            _mapsSerializeSystem = GetComponent<MapSerializeSystem>();
            _currentMaps = new List<GameObject>();  
        }

        public void LoadMap(string map)
        {
            if(_current != null)
            {
                ConsoleProDebug.LogToFilter($"{_currentMapObjects.Count} MapObjects returned to pool", "MapSerialization");
                _currentMapObjects.ForEach(obj => PoolSystem.Instance.ReturnToPool(obj));
                _current.SetActive(false);
            }

            _current = _currentMaps.Find(mapRow => Util.RemoveCloneTag(mapRow.name).Equals(map));
            if(_current == null)
            {
                ConsoleProDebug.LogAsType($"You're trying to load map by name {map} which DOESN'T EXIST!", "Error");
                return;
            }

            MapSerializeDataWrapper data = _mapsSerializeSystem.Load(map);
            
            ConsoleProDebug.LogToFilter($"Current map: {_current.name}", "MapSerialization");
            ConsoleProDebug.LogToFilter($"Current map without (Clone): {Util.RemoveCloneTag(_current.name)}", "MapSerialization");
            ConsoleProDebug.LogToFilter($"Map name: {Util.RemoveCloneTag(map)}", "MapSerialization");

            _current.SetActive(true);

            _navMeshSurface.BuildNavMesh();

            data.serializableObjects.ForEach(obj =>
            {
                GameObject trigger = PoolSystem.Instance.SpawnFromPool(obj.name);
                trigger.GetComponent<ISerializableObject>().Initialize(obj.key, ES3Settings.defaultSettings);
                trigger.GetComponent<ISerializableObject>().Load(ES3Settings.defaultSettings, _mapsSerializeSystem.GetMapJsonPath(map));
                _currentMapObjects.Add(trigger);
            });

            //Set Grid
            // _gridSystem.Grid = _current.GetComponentInChildren<Autotiles3D_Grid>();

            //Post Processing Profile
            _currentVolume.profile = _resourcesSystem.LoadVolumeProfile(data.volume);
            ConsoleProDebug.LogToFilter($"profile: {_resourcesSystem.LoadVolumeProfile(data.volume)}", "MapSerialization");
            ConsoleProDebug.LogToFilter($"current profile: {_currentVolume.profile}", "MapSerialization");

            //Camera background color
            if(ColorUtility.TryParseHtmlString(data.skyColor, out Color backgroundColor))
                Camera.main.backgroundColor = backgroundColor;

            //Camera Effects
            // _cameraSystem.SetEffect(data.effect);

            //Directional Light Settings
            if(ColorUtility.TryParseHtmlString(data.lightData.filter, out Color lightColor))
            {
                _mainLight.color = lightColor;
                _mainLight.colorTemperature = data.lightData.temperature;
                _mainLight.intensity = data.lightData.intensity;
            }
            
            if(_debug)
            {
                ConsoleProDebug.LogToFilter($"Current {_current}", "Player");
                ConsoleProDebug.LogToFilter($"Current Volume {_currentVolume}", "MapSerialization");
            }
            // OnMapLoaded.Invoke();
        }

        public void LoadFloor(string floor)
        {
            _currentFloor = floor;

            FloorSerializableObject data = _mapsSerializeSystem.LoadFloor(floor);

            foreach (FloorPositionalData posData in data.positionalData)
            {
                // Maybe make Config injectable?
                // string mapPath = Config.Instance.MapPrefabsPath + posData.mapId;
                // GameObject prefab = Resources.Load<GameObject>(mapPath);
                // GameObject mapClone = Instantiate(prefab, posData.position, Quaternion.identity, _floorParent);
                // mapClone.SetActive(false);
                // _currentMaps.Add(mapClone);
            }

            _current = _currentMaps[0];
        }

        public void Teleport(string map, Vector3 coordinates)
        {
            LoadMap(map);
            _player.Teleport(coordinates);
        }

        public void Teleport(Vector3 coordinates)
        {
            _player.Teleport(coordinates);
        }

        #region Serialization
        public void Save(ES3Settings settings)
        {
            ES3.Save("CurrentMap", Util.RemoveCloneTag(_current.name), settings);
            ES3.Save("CurrentFloor", _currentFloor, settings);
        }
        #endregion
    }
}