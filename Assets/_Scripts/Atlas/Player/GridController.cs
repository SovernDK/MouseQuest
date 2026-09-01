using System.Collections;
using Atlas.AI.Grid;
using Atlas.Effects;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GridNav
{
    public class GridController : MonoBehaviour
    {
        // [Inject] 
        // private GridSystem _gridSystem;
        // [Inject]
        // public EffectsSystem _effects;
        
        // [SerializeField]
        // private float _movementDuration;
        // [SerializeField]
        // private float _jumpPower;
        // [SerializeField]
        // private Vector3Int _startingPos;
        // [SerializeField]
        // private Transform _sprite;

        // private Vector3Int _currentCoordinates;
        // private Vector3 _spritePosition;
        // private bool _canMove = true;

        // public Vector3Int CurrentCoordinates { get => _currentCoordinates; set => _currentCoordinates = value; }

        // private void Start() 
        // {
        //     // Spawn();
        //     _spritePosition = _sprite.localPosition;
        // }

        // public IEnumerator OnUpdate() 
        // {
        //     ProcessCommand();
        //     yield return null;
        // }

        // public void ProcessCommand()
        // {
        //     if(!_canMove) return;

        //     if(Input.GetAxisRaw("Horizontal") == 1) MoveToTile(Vector3Int.right);
        //     else if(Input.GetAxisRaw("Horizontal") == -1) MoveToTile(Vector3Int.left);
        //     else if(Input.GetAxisRaw("Vertical") == 1) MoveToTile(Vector3Int.forward);
        //     else if(Input.GetAxisRaw("Vertical") == -1) MoveToTile(Vector3Int.back);
        // }

        // private void Update() 
        // {
        //     ProcessCommand();
        // }

        // public void Spawn()
        // {
        //     _currentCoordinates = _startingPos;
        //     transform.position = _gridSystem.GetWorld(_currentCoordinates);
        // }

        // public void MoveToTile(Vector3Int direction)
        // {
        //     Atlas.Tile currentTile = _gridSystem.GetBlock(_currentCoordinates).GetComponent<Atlas.Tile>();
        //     if(currentTile.IsMovementBlocked(direction)) return;
           
        //     Vector3Int target = _currentCoordinates + currentTile.GetShiftByDirection(direction);
        //     Autotiles3D_BlockBehaviour targetBlock = _gridSystem.GetFirstClimableBlock(target);

        //     if(targetBlock == null) return;

        //     Atlas.Tile targetTile = targetBlock.GetComponent<Atlas.Tile>();

        //     if(targetTile == null) return;

        //     Vector3 directionOfApproach = _currentCoordinates - targetBlock.InternalPosition;
        //     directionOfApproach = directionOfApproach.normalized;
            
        //     //there is a bug where if there is no tile on pos it will throw null ref exc and stops PlayerFSM from updating
        //     if(targetTile.IsDirectionBlocked(Vector3Int.RoundToInt(directionOfApproach))) return;

        //     PlayMove(targetTile.Anchor);
        //     _currentCoordinates = targetBlock.InternalPosition;
        // }

        // public void PlayMove(Vector3 newPosition)
        // {
        //     _canMove = false;
        //     // _effects.AddSoundEffect(Gamemaster.Instance.Config.StepSfx.name, newPosition, Quaternion.identity, 0f, transform, true);
        //     transform.DOMove(newPosition, _movementDuration);
        //     _sprite.DOPunchScale(new Vector3(-.1f, .1f, 0f), _movementDuration);
        //     _sprite.DOLocalJump(_spritePosition, _jumpPower, 1, _movementDuration)
        //                 .OnComplete(() => { _canMove = true; });
        // }

        // public void Teleport(Vector3Int target)
        // {
        //     Autotiles3D_BlockBehaviour targetBlock = _gridSystem.GetFirstClimableBlock(target);

        //     ConsoleProDebug.LogToFilter($"Target to {targetBlock.InternalPosition}", "Player");
        //     if(targetBlock == null) return;

        //     Atlas.Tile targetTile = targetBlock.GetComponent<Atlas.Tile>();
        //     Debug.Log($"TargetTile {targetTile}");
        //     PlayMove(targetTile.Anchor);
        //     _currentCoordinates = targetBlock.InternalPosition;
        //     ConsoleProDebug.LogToFilter($"Teleported to {target}", "Player");
        // }
    }
}