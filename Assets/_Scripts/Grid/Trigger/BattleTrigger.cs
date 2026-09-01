using UnityEngine;
using Zenject;

public class BattleTrigger : OverworldTrigger
{
    [SerializeField] [ES3Serializable]
    private int _battleId;    
    [SerializeField] [ES3Serializable]
    private SpriteRenderer _sprite;
    [SerializeField] [ES3Serializable]
    private bool _alive = true;

    [Inject]
    private BattleSystem _battleSystem;

    public override void Trigger()
    {
        base.Trigger();
        
        _battleSystem.StartBattle(_battleId);
        _alive = false;

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<GridPawn>().enabled = false;
        GetComponent<OverworldEnemyBehaviour>().enabled = false;

        _triggerable = false;
    }

    public void OnObjectSpawned()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _originalPosition = Vector3.zero;
    }

    public class Factory : PlaceholderFactory<BattleTrigger> { }
}

