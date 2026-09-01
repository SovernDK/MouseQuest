using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Atlas.Core 
{
    public class StagingSystem : MonoBehaviour
    {
        public UnityEvent<string> OnGameSceneStaged;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Resources.UnloadUnusedAssets();
            OnGameSceneStaged.Invoke(scene.name);
        }

        public void StageScene(int id)
        {
            SceneManager.LoadScene(id);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}