using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas 
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] [InlineProperty] [HideLabel] [TitleGroup("Movement Directions")]
        private List<MovementDirections> _movementDirections = new List<MovementDirections>()
        {
            new MovementDirections(Vector3Int.forward, Vector3Int.forward, false),
            new MovementDirections(Vector3Int.back, Vector3Int.back, false),
            new MovementDirections(Vector3Int.left, Vector3Int.left, false),
            new MovementDirections(Vector3Int.right, Vector3Int.right, false),
        };

        [SerializeField] [InlineProperty] [HideLabel] [TitleGroup("Blocking Directions")]
        private List<BlockingDirections> _blockingDirections = new List<BlockingDirections>()
        {
            new BlockingDirections(Vector3Int.forward, false),
            new BlockingDirections(Vector3Int.back, false),
            new BlockingDirections(Vector3Int.left, false),
            new BlockingDirections(Vector3Int.right,  false),
            new BlockingDirections(new Vector3Int(0, -1, 1),  false),
            new BlockingDirections(new Vector3Int(0, -1, -1),  false),
            new BlockingDirections(new Vector3Int(1, -1, 0),  false),
            new BlockingDirections(new Vector3Int(-1, -1, 0),  false),
            new BlockingDirections(new Vector3Int(0, 1, 1),  false),
            new BlockingDirections(new Vector3Int(0, 1, -1),  false),
            new BlockingDirections(new Vector3Int(1, 1, 0),  false),
            new BlockingDirections(new Vector3Int(-1, 1, 0),  false),
        };
        
        [SerializeField]
        private Transform _anchor;

        public Vector3 Anchor { get => _anchor.position; }

        public bool IsMovementBlocked(Vector3Int direction)
        {
            return _movementDirections.Find(_mov => _mov.Direction == transform.InverseTransformDirection(direction)).Blocked;
        }

        public bool IsDirectionBlocked(Vector3Int directionOfApproach)
        {
            return _blockingDirections.Find(_mov => _mov.Direction == transform.InverseTransformDirection(directionOfApproach)).Blocked;
        }

        public Vector3Int GetShiftByDirection(Vector3Int direction)
        {
            return _movementDirections.Find(_mov => _mov.Direction == direction).Shift;
        }

        [Button("Block All")]
        public void BlockAll()
        {
            _blockingDirections.ForEach(dir => 
            {
                dir.Blocked = true;
            });
        }

        private void OnDrawGizmos() 
        {
            if(_anchor == null) return;
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(_anchor.position, .1f);    
        }
    }

    [Serializable]
    public class MovementDirections
    {
        [SerializeField] [ReadOnly]
        private Vector3Int _direction;
        [SerializeField] [HideIf("@_blocked == true")]
        private Vector3Int _shift;
        [SerializeField]
        private bool _blocked;
            
        public Vector3Int Direction { get => _direction; set => _direction = value; }
        public Vector3Int Shift { get => _shift; set => _shift = value; }
        public bool Blocked { get => _blocked; set => _blocked = value; }

        public MovementDirections(Vector3Int _direction, Vector3Int _shift, bool blocked)
        {
            this._direction = _direction;
            this._shift = _shift;
            this._blocked = blocked;
        }
    }

    [Serializable]
    public class BlockingDirections
    {
        [SerializeField] [ReadOnly] [HideLabel] [HorizontalGroup("Blocking")]
        private Vector3Int _direction;
        [SerializeField] [HorizontalGroup("Blocking")]
        private bool _blocked;
            
        public Vector3Int Direction { get => _direction; set => _direction = value; }
        public bool Blocked { get => _blocked; set => _blocked = value; }

        public BlockingDirections(Vector3Int _direction, bool blocked)
        {
            this._direction = _direction;
            this._blocked = blocked;
        }
    }
}