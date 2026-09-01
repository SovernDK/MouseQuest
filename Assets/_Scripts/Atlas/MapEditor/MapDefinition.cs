using Atlas.Core.Serialization;

using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using System.IO; // Required for working with prefabs in the editor

namespace Atlas.Editor 
{
    public class MapDefinition : MonoBehaviour
    {
        [SerializeField]
        private MapSerializeSystem _mapSerializeSystem;

        [SerializeField]
        private string _saveFolder = "Assets/Resources/Maps/Prefabs"; // Default save folder

        [SerializeField]
        private Vector3 boundSize;

        [Button("Update")]
        public void UpdateMap()
        {
            string volumeName = FindFirstObjectByType<Volume>().profile.name;
            string colorHex = Camera.main.backgroundColor.ToHexString();

            Light mainLight = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>();

            MapLightData lightData = new MapLightData()
            {
                filter = '#' + mainLight.color.ToHexString(),
                temperature = mainLight.colorTemperature,
                intensity = mainLight.intensity
            };
            Vector3 position = transform.position + new Vector3(20, 0, 20);
            Vector3 bounds = new Vector3(40, 20, 40);
            Bounds saveArea = new Bounds(position, bounds);

            _mapSerializeSystem.Save(name, volumeName, colorHex, false, lightData, saveArea);

            SaveAsPrefab();
        }

        private void SaveAsPrefab()
        {
    #if UNITY_EDITOR
            // Ensure the save folder exists
            if (!AssetDatabase.IsValidFolder(_saveFolder))
            {
                string[] folders = _saveFolder.Split('/');
                string path = "";
                foreach (string folder in folders)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!AssetDatabase.IsValidFolder(path + "/" + folder))
                        {
                            AssetDatabase.CreateFolder(path, folder);
                        }
                        path += "/" + folder;
                    }
                    else
                    {
                        path = folder;
                    }
                }
            }

            string prefabPath = $"{_saveFolder}/{name}.prefab";

            GameObject tempObject = new GameObject(name);
            

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out MapAnchor anchor) && anchor.IsMerged)
                {
                    // Create a new GameObject for merged objects
                    GameObject copiedChild = new GameObject(child.name);
                    copiedChild.transform.SetParent(tempObject.transform);

                    // Transfer position, rotation, and scale
                    copiedChild.transform.localPosition = child.localPosition;
                    copiedChild.transform.localRotation = child.localRotation;
                    copiedChild.transform.localScale = child.localScale;

                    // Copy MeshFilter and MeshRenderer components
                    if (child.TryGetComponent(out MeshFilter meshFilter))
                    {
                        MeshFilter newMeshFilter = copiedChild.AddComponent<MeshFilter>();
                        newMeshFilter.sharedMesh = meshFilter.sharedMesh;
                    }

                    if (child.TryGetComponent(out MeshRenderer meshRenderer))
                    {
                        MeshRenderer newMeshRenderer = copiedChild.AddComponent<MeshRenderer>();
                        newMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;
                    }

                    if (child.TryGetComponent(out MeshCollider meshCollider))
                    {
                        MeshCollider newMeshCollider = copiedChild.AddComponent<MeshCollider>();
                        newMeshCollider = meshCollider;
                    }
                }
                else
                {
                    // Copy child as is for non-merged objects or if MapAnchor is absent
                    Transform copiedChild = Instantiate(child, tempObject.transform);
                    copiedChild.name = child.name;
                }
            }

            // Check if the prefab already exists
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (existingPrefab != null)
            {
                // Override the existing prefab
                PrefabUtility.SaveAsPrefabAssetAndConnect(tempObject, prefabPath, InteractionMode.AutomatedAction);
                Debug.Log($"Prefab '{name}' updated at '{prefabPath}'");
            }
            else
            {
                // Create a new prefab
                PrefabUtility.SaveAsPrefabAsset(tempObject, prefabPath);
                Debug.Log($"Prefab '{name}' created at '{prefabPath}'");
            }

            // Clean up the temporary object
            DestroyImmediate(tempObject);
    #else
            Debug.LogWarning("Prefab saving is only available in the Unity Editor.");
    #endif
        }

        [ContextMenu("Merge Child Meshes")]
        public void MergeMeshes()
        {
            foreach(MapAnchor anchor in GetComponentsInChildren<MapAnchor>())
            {
                if(anchor.allowMerging) MergeMesh(anchor);
            }
        }

        public void MergeMesh(MapAnchor anchor)
        {
            if (anchor.IsMerged)
            {
                Debug.LogWarning("Meshes are already merged. Unmerge before merging again.");
                return;
            }

            // Get all MeshFilter components in children
            MeshFilter[] meshFilters = anchor.gameObject.GetComponentsInChildren<MeshFilter>();

            if (meshFilters.Length <= 1)
            {
                Debug.LogWarning("No child meshes found to merge.");
                return;
            }

            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            Material[] materials = new Material[meshFilters.Length];
            int index = 0;

            foreach (MeshFilter mf in meshFilters)
            {
                if (mf.gameObject == gameObject)
                    continue;

                MeshRenderer renderer = mf.GetComponent<MeshRenderer>();
                if (renderer != null && mf.sharedMesh != null)
                {
                    combine[index].mesh = mf.sharedMesh;
                    combine[index].transform = Matrix4x4.TRS(
                        mf.transform.position - anchor.transform.position,
                        mf.transform.rotation,
                        mf.transform.lossyScale
                    );
                    materials[index] = renderer.sharedMaterial;
                    mf.gameObject.SetActive(false); // Disable the child GameObject after merging
                    index++;
                }
            }

            MeshFilter parentMeshFilter = anchor.gameObject.GetComponent<MeshFilter>();
            MeshRenderer parentMeshRenderer = anchor.gameObject.GetComponent<MeshRenderer>();

            // Save the original mesh
            anchor.OriginalMesh = parentMeshFilter.sharedMesh;

            // Combine the meshes into one
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine, true, true);
    #if UNITY_EDITOR
            string meshPath = Path.Combine("Assets", "Resources", "MergedMeshes", $"{EditorSceneManager.GetActiveScene().name}", $"{gameObject.name}");
            
            EnsureFoldersExist(meshPath);

            string fileName = $"{anchor.name}_CombinedMesh.asset";
            string fullPath = Path.Combine(meshPath, fileName);

            AssetDatabase.CreateAsset(combinedMesh, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
    #endif
            parentMeshFilter.mesh = combinedMesh;

            // Assign the materials (if needed)
            if (materials.Length > 0)
            {
                parentMeshRenderer.sharedMaterial = materials[0]; // Use the first material (for simplicity)
            }

            anchor.AddComponent<MeshCollider>();

            anchor.IsMerged = true;
            Debug.Log("Mesh merging completed!");
        }

        [ContextMenu("Unmerge Meshes")]
        public void UnmergeMeshes()
        {
            foreach(MapAnchor anchor in GetComponentsInChildren<MapAnchor>())
            {
                if(anchor.allowMerging) UnmergeMesh(anchor);
            }
        }

        public void UnmergeMesh(MapAnchor anchor)
        {
            if (!anchor.IsMerged)
            {
                Debug.LogWarning("Meshes are not merged. Nothing to unmerge.");
                return;
            }

            MeshFilter parentMeshFilter = anchor.gameObject.GetComponent<MeshFilter>();

            // Restore the original mesh
            parentMeshFilter.mesh = anchor.OriginalMesh;

            // Reactivate child GameObjects
            MeshFilter[] meshFilters = anchor.gameObject.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter mf in meshFilters)
            {
                if (mf.gameObject != gameObject)
                {
                    mf.gameObject.SetActive(true);
                }
            }

            DestroyImmediate(anchor.gameObject.GetComponent<MeshCollider>());

            anchor.IsMerged = false;
            Debug.Log("Mesh unmerging completed!");
        }

        #if UNITY_EDITOR
        private static void EnsureFoldersExist(string fullPath)
        {
            // Start from "Assets" and build the folder structure incrementally
            string[] parts = fullPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string currentPath = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string nextPath = Path.Combine(currentPath, parts[i]);

                // Create folder if it doesn't exist
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }

                currentPath = nextPath;
            }
        }
        #endif

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.red;
            Vector3 position = transform.position + boundSize / 2;
            Vector3 bounds = boundSize;
            Gizmos.DrawWireCube(position, bounds);
        } 
    }
}
