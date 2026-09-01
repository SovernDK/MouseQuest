using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloorSerializableObject
{
    public string name;
    public List<FloorPositionalData> positionalData;
}

[Serializable]
public class FloorPositionalData
{
    public string mapId;
    public Vector3Int position;
}
