using UnityEngine;

public class OverworldEnemyBehaviour : MonoBehaviour
{
    [SerializeField] [ES3Serializable]
    private float _luringRange;
    [SerializeField] [ES3Serializable]
    private float _detectionRange;
    [SerializeField] [ES3Serializable]
    private LayerMask _layerMask;
    [ES3NonSerializable]
    private GridPawn _pawn;
    [ES3Serializable]
    private Vector3 _nestingPosition;

    //OPTIMIZE IT
    private void Start() 
    {
        _nestingPosition = transform.position;    
        _pawn = GetComponent<GridPawn>();
    }

    private void Update() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRange, _layerMask);
        if(colliders.Length > 0)
        {
            _pawn.SetTarget(colliders[0].transform.position);
        }
        else
        {
            _pawn.SetTarget(_nestingPosition);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_nestingPosition, .1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
