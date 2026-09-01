using System;
using System.IO;
using Atlas.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Atlas.Core.Serialization
{
    public class Serialize : MonoBehaviour, ISerializableObject
    {  
        protected string _serializationKey;
        protected ES3Settings _settings;

        [SerializeField] [ES3NonSerializable]
        private bool _saveChildren;

        public string SerializationKey { get => _serializationKey; set => _serializationKey = value; }
        
        public UnityEvent Loaded;

        #region ISerializable
        public void Initialize(string key, ES3Settings settings)
        {
            _serializationKey = key;
            _settings = settings;
            _settings.saveChildren = _saveChildren;
        }

        public virtual void Save(ES3Settings settings, string path = "")
        {
            try
            {
                ES3.Save($"_go_{_serializationKey}", gameObject, path, _settings);
            }
            catch (Exception)
            {
                
            }
        }

        public virtual void Load(ES3Settings settings, string path = "")
        {
            try
            {
                ES3.LoadInto($"_go_{_serializationKey}", path, gameObject, _settings);
                Loaded.Invoke();
            }
            catch (Exception)
            {
                
            }
        }

        public virtual SerializedObject GetSerializedObject()
        {
            return new SerializedObject()
            {
                name = Util.RemoveNumberFromDuplicatedName(gameObject.name),
                key = _serializationKey,
            };
        }
        #endregion
    }
}