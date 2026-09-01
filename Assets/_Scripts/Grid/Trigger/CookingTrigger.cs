using Atlas.Core;
using Atlas.Core.Serialization;
using UnityEngine;
using Atlas.Utility;
using Atlas.Pooling;
using Zenject;

public class CookingTrigger : OverworldTrigger
{
    #region IPoolable
    public string Name => gameObject.name;
    public GameObject Prefab => gameObject;
    public int AmountToPool => 5;
    public bool ShouldExpand => true;
    public bool LazyPool => false;
    #endregion

    [Inject]
    private CookingSystem _cooking;

    // public override void Load(MapSerializedObject saveData, string path)
    // {
    //     base.Load(saveData);
    // }

    // public override MapSerializedObject Save(string id, string path)
    // {
    //     return new MapSerializedObject()
    //     {
    //         name = Util.RemoveNumberFromDuplicatedName(gameObject.name),
    //         objectId = id
    //     };
    // }

    public override void Trigger()
    {
        base.Trigger();
        _cooking.Open();
    }

    public void OnObjectSpawned()
    {
        _originalPosition = _icon.GetComponent<RectTransform>().position;
    }
}
