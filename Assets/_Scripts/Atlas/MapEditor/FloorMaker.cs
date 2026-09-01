using System.Collections;
using System.Collections.Generic;
using Atlas.Core.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MapSerializeSystem))]
public class FloorMaker : MonoBehaviour
{
    public string floorName;

    [Button("SaveOrUpdate")]
    public void SaveOrUpdate()
    {
        MapSerializeSystem serializeSystem = GetComponent<MapSerializeSystem>();
        List<FloorPositionalData> floorPositionalData = new List<FloorPositionalData>();    
        foreach(Transform child in transform)
        {
            floorPositionalData.Add(new FloorPositionalData() { mapId = child.name, position = Vector3Int.RoundToInt(child.position) }); 
        }
        Debug.Log($"floorPositionalData {floorPositionalData.Count}");

        serializeSystem.SaveFloor(floorName, floorPositionalData);
    }
}
