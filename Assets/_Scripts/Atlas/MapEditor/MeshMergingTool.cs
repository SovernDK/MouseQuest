#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshMergingTool : MonoBehaviour
{
    private Mesh originalMesh;
    private bool isMerged = false;

    [ContextMenu("Merge Child Meshes")]
    public void MergeMeshes()
    {
        if (isMerged)
        {
            Debug.LogWarning("Meshes are already merged. Unmerge before merging again.");
            return;
        }

        // Get all MeshFilter components in children
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

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
                combine[index].transform = mf.transform.localToWorldMatrix;
                materials[index] = renderer.sharedMaterial;
                mf.gameObject.SetActive(false); // Disable the child GameObject after merging
                index++;
            }
        }

        MeshFilter parentMeshFilter = GetComponent<MeshFilter>();
        MeshRenderer parentMeshRenderer = GetComponent<MeshRenderer>();

        // Save the original mesh
        originalMesh = parentMeshFilter.sharedMesh;

        // Combine the meshes into one
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);
#if UNITY_EDITOR
        string meshPath = $"Assets/Resources/MergedMeshes/{EditorSceneManager.GetActiveScene().name}/{gameObject.name}_CombinedMesh.asset";
        if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/MergedMeshes"))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "MergedMeshes");
        }
        UnityEditor.AssetDatabase.CreateAsset(combinedMesh, meshPath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
        parentMeshFilter.mesh = combinedMesh;

        // Assign the materials (if needed)
        if (materials.Length > 0)
        {
            parentMeshRenderer.sharedMaterial = materials[0]; // Use the first material (for simplicity)
        }

        isMerged = true;
        Debug.Log("Mesh merging completed!");
    }

    [ContextMenu("Unmerge Meshes")]
    public void UnmergeMeshes()
    {
        if (!isMerged)
        {
            Debug.LogWarning("Meshes are not merged. Nothing to unmerge.");
            return;
        }

        MeshFilter parentMeshFilter = GetComponent<MeshFilter>();

        // Restore the original mesh
        parentMeshFilter.mesh = originalMesh;

        // Reactivate child GameObjects
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.gameObject != gameObject)
            {
                mf.gameObject.SetActive(true);
            }
        }

        isMerged = false;
        Debug.Log("Mesh unmerging completed!");
    }
}
#endif