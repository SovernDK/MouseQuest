using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Atlas.Player;

namespace Atlas.Core.Serialization
{
    public class SerializationSystem : MonoBehaviour
    {
        [Inject(Optional = true)]
        PlayerSystem Player { get; set; }

        private ES3Settings _settings;

        private void OnEnable() 
        {
            ConsoleProDebug.LogToFilter($"Serialization System Enabled", "SerializationSystem");
            _settings = new ES3Settings(ES3.Location.Cache);  
        }

        public void Save()
        {
            IEnumerable<ISerializableObject> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, 
                                                                                    FindObjectsSortMode.InstanceID)
                                                            .OfType<ISerializableObject>();

            foreach (ISerializableObject saveable in saveables)
            {
                saveable.Save(_settings);
            }

            ES3.StoreCachedFile();
        }

        public void Load()
        {
            SerializationDataWrapper serializationDataWrapper = new SerializationDataWrapper();
            IEnumerable<ISerializableObject> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, 
                                                                                    FindObjectsSortMode.InstanceID)
                                                            .OfType<ISerializableObject>();

            foreach (ISerializableObject saveable in saveables)
            {
                saveable.Load(_settings);
            }
        }
    }

    public class SerializationDataWrapper
    {
        public List<SerializedObject> serializableObjects = new List<SerializedObject>();
    }

    public interface ISerializableObject
    {
        public string SerializationKey { get; set; }
        void Initialize(string key, ES3Settings settings);
        void Save(ES3Settings settings, string path = "");
        void Load(ES3Settings settings, string path = "");
        SerializedObject GetSerializedObject();
    }

    [Serializable]
    public class SerializedObject
    {
        public string name;
        public string key;
    }
}
