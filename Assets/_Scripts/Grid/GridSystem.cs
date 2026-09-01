using System;
using UnityEngine;
using System.Collections.Generic;

namespace Atlas.AI.Grid
{
    public class GridSystem : MonoBehaviour
    {
        // [SerializeField]
        // private Autotiles3D_Grid _grid;
        // private List<Node> _nodes;

        // public Autotiles3D_Grid Grid { get => _grid; set => _grid = value; }

        // public Vector3 GetWorld(Vector3Int coordinate)
        // {
        //     return Grid.ToWorldPoint(coordinate) * Grid.Unit;
        // }

        // public Autotiles3D_BlockBehaviour GetBlock(Vector3Int coordinate)
        // {
        //     if(Grid.GetBlocks(coordinate).Count > 0)
        //         return Grid.GetBlocks(coordinate)[0];

        //     return null;
        // }

        // public bool CheckBlock(Vector3Int coordinate)
        // {
        //     coordinate.y += 1;
        //     return Grid.GetBlocks(coordinate).Count > 0;
        // }

        // public Autotiles3D_BlockBehaviour GetFirstClimableBlock(Vector3Int target)
        // {
        //     for(int i = 1; i > -2; i--)
        //     {
        //         List<Autotiles3D_BlockBehaviour> checkedBlocks = Grid.GetBlocks(new Vector3Int(target.x, target.y + i, target.z));
        //         foreach(Autotiles3D_BlockBehaviour tile in checkedBlocks)
        //         {
        //             if(tile != null && !tile.CompareTag("Ignore"))
        //             {
        //                 if(checkedBlocks[0].CompareTag("Walkable")) return checkedBlocks[0];
        //                 else return null;
        //             }
        //         }
        //     }

        //     return null;
        // }
    }

    [Serializable]
    public class Node
    {
        private Vector3Int coordinates;
        private Vector3Int position;
        private bool isOccupied;

        public Vector3Int Coordinates { get => coordinates; set => coordinates = value; }
        public Vector3Int Position { get => position; set => position = value; }
        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    }
}