using UnityEngine;
using UnityEngine.AI;

public class GridPawn : MonoBehaviour
{
    [SerializeField] [ES3NonSerializable]
    private NavMeshAgent _agent;

    [SerializeField] [ES3NonSerializable]
    private Vector3 _target;
    private float searchRadius = 50.0f;

    public Vector3 Target { get => _target; set => _target = value; }

    private void Start() 
    {
        _agent.updateRotation = false;
    }

    private void Update() 
    {
        if(_target != null) 
            _agent.SetDestination(_target);
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    Vector3 GetClosestNavMeshPosition(Vector3 currentPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(currentPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            return hit.position; // Valid point on the NavMesh
        }
        else
        {
            Debug.LogError("Failed to find a valid position on the NavMesh near the current position.");
            return currentPosition; // Fallback: Use the original position
        }
    }

    public void UpdateAgent()
    {
        _agent.enabled = true;
        _agent.Warp(GetClosestNavMeshPosition(transform.position));
    }

    public void DisableAgent() 
    {
        _agent.enabled = false;
    }

    public void UpdateAgentPosition(Vector3 targetPosition)
    {
        _agent.Warp(GetClosestNavMeshPosition(targetPosition));
    }
}
