using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.Editor
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class MapAnchor : MonoBehaviour
    {
        public bool allowMerging;
        
        [SerializeField] [ReadOnly]
        private bool _isMerged;
        [SerializeField]
        private Mesh _originalMesh;

        public bool IsMerged { get => _isMerged; set => _isMerged = value; }
        public Mesh OriginalMesh { get => _originalMesh; set => _originalMesh = value; }
    }
}