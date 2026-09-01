using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Atlas.Core.Serialization 
{
    public class MapSerializeSystem : MonoBehaviour
    {
        private ES3Settings _settings;

        public void Save(string mapName, string volume, string skyColorHex, bool effect, MapLightData data, Bounds bounds)
        {
            _settings = ES3Settings.defaultSettings;

            //// Create folder and FILE
            string saveDirectory = Path.Combine(Application.streamingAssetsPath, "Maps");
            string filePath = Path.Combine(saveDirectory, $"{mapName}.json");

            ES3.DeleteFile(filePath);

            // if (!Directory.Exists(saveDirectory))
            // {
            //     Directory.CreateDirectory(saveDirectory);
            //     ConsoleProDebug.LogToFilter("Created save directory: " + saveDirectory, "MapSerialization");
            // }

            //// Base map data
            MapSerializeDataWrapper saveDataWrapper = new MapSerializeDataWrapper
            {
                mapName = mapName,
                volume = volume,
                skyColor = '#' + skyColorHex,
                effect = effect,
                lightData = data
            };

            //// Gather objects name and ID's
            IEnumerable<ISerializableObject> saveables = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include, FindObjectsSortMode.InstanceID
            ).OfType<ISerializableObject>();

            int uniqeId = 0;
            foreach (ISerializableObject saveable in saveables)
            {
                if (saveable is MonoBehaviour monoBehaviour)
                {
                    if (bounds.Contains(monoBehaviour.transform.position) && saveable is Serialize)
                    {
                        string serializationKey = $"{uniqeId}{mapName}";
                        saveable.Initialize(serializationKey, _settings);
                        saveDataWrapper.serializableObjects.Add(saveable.GetSerializedObject());
                        saveable.Save(_settings, filePath);
                        
                        uniqeId++;
                    }
                }
            }

            //// Finally save it to FILE
            ES3.Save($"{mapName}", saveDataWrapper, filePath);

            ConsoleProDebug.LogToFilter("Saved to: " + filePath, "MapSerialization");
        }

        public void SaveFloor(string name, List<FloorPositionalData> floorPositionalData)
        {
            FloorSerializableObject floor = new FloorSerializableObject() 
            {
                name = name,
                positionalData = floorPositionalData
            };

            string saveDirectory = Path.Combine(Application.streamingAssetsPath, "Floors");
            string filePath = Path.Combine(saveDirectory, name+".json");

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                ConsoleProDebug.LogToFilter("Created save directory: " + saveDirectory, "MapSerialization");
            }

            string content = JsonUtility.ToJson(floor, true);

            File.WriteAllText(filePath, content);
            ConsoleProDebug.LogToFilter("Saved to: " + filePath, "MapSerialization");
        }

        public MapSerializeDataWrapper Load(string mapName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Maps");
            
            MapSerializeDataWrapper wrapper = ES3.Load<MapSerializeDataWrapper>($"{mapName}", $"{filePath}/{mapName}.json");
            
            ConsoleProDebug.LogToFilter($"Loaded from: {filePath}/{mapName}.json", "MapSerialization");
            return wrapper;
        }

        public FloorSerializableObject LoadFloor(string floorName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Floors", $"{floorName}.json");
            string json = File.ReadAllText(filePath);

            FloorSerializableObject wrapper = JsonUtility.FromJson<FloorSerializableObject>(json);
            
            ConsoleProDebug.LogToFilter("Loaded from: " + filePath, "MapSerialization");
            return wrapper;
        }

        public string GetMapJsonPath(string mapName)
        {
            return Path.Combine(Application.streamingAssetsPath, "Maps", $"{mapName}.json");
        }
    }

    public class MapSerializeDataWrapper
    {
        public string mapName;
        public string volume;
        public string skyColor;
        public bool effect;
        public MapLightData lightData;
        public List<SerializedObject> serializableObjects = new List<SerializedObject>();
    }

    [Serializable]
    public class MapLightData
    {
        public string filter;
        public float temperature;
        public float intensity;
    }

    public interface IMapSerializableObject
    {
        string Key { get; set; }
        MapSerializedObject GetSerializationObjectData();
        MapSerializedObject Save(string id, string path = "");
        void Load(MapSerializedObject saveData, string path = "");
    }

    [Serializable]
    public class MapSerializedObject
    {
        public string name;
        public string objectId;
    }

}