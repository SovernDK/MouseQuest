using Atlas.Map;
using UnityEngine;
using Zenject;

public class TeleportTrigger : OverworldTrigger
{
    [SerializeField] [ES3Serializable]
    private string _map;
    [SerializeField] [ES3Serializable]
    private Vector3 _coordinates;

    [Inject]
    private MapSystem _mapSystem;

    public override void Trigger()
    {
        base.Trigger();
        _mapSystem.Teleport(_map, _coordinates);
    }

    public void OnObjectSpawned()
    {
        
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_coordinates, 1f);
        Gizmos.DrawLine(transform.position, _coordinates);
    }

    public class Factory : PlaceholderFactory<TeleportTrigger> {}
}