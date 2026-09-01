using Atlas.Core.Serialization;

using UnityEngine;
using Atlas.Utility;
using Zenject;
using Atlas.Core;
using System.Runtime.InteropServices;

public class ShopTrigger : OverworldTrigger
{
    [SerializeField]
    private int _shopId;

    [Inject]
    private ShopSystem _shopSystem;

    #region IPoolable
    public string Name => gameObject.name;
    public GameObject Prefab => gameObject;
    public int AmountToPool => 5;
    public bool ShouldExpand => true;
    public bool LazyPool => false;
    #endregion

    private class ShopData
    {
        public int ShopId { get; set; }
    }

    public override void Trigger()
    {
        base.Trigger();
        _shopSystem.Open(_shopId);
    }

    // public override void Load(MapSerializedObject saveData, string path)
    // {
    //     base.Load(saveData);

    //     // if(saveData.data != null)
    //     // {
    //     //     ShopData data = JsonUtility.FromJson<ShopData>(saveData.data);

    //     //     _shopId = data.ShopId;
    //     // }
    // }

    // public override MapSerializedObject Save(string id, string path)
    // {
    //     ShopData data = new ShopData()
    //     {
    //         ShopId = _shopId,
    //     };

    //     return new MapSerializedObject()
    //     {
    //         name = Util.RemoveNumberFromDuplicatedName(gameObject.name),
    //         objectId = id.ToString(),//GetObjectId(),
    //         // type = "ShopTrigger",
    //         // position = transform.position,
    //         // rotation = transform.rotation,
    //         // data = JsonUtility.ToJson(data)
    //     };
    // }

    public void OnObjectSpawned()
    {
        
    }
}
