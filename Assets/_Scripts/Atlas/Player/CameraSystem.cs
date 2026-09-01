using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.Player
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField]
        private Camera _battleCamera;

        [SerializeField]
        private Transform _target;
        [SerializeField]
        private Vector3 _positionOffset;
        [SerializeField]
        private Vector3 _rotationOffset;
        [SerializeField]
        private float _smoothing;

        [TitleGroup("Shake")] [SerializeField]
        private float _duration;
        [TitleGroup("Shake")] [SerializeField]
        private float _strength;
        [TitleGroup("Shake")] [SerializeField]
        private int _vibrato;

        [TitleGroup("Effects")] [SerializeField]
        private GameObject _godRays;

        void Update()
        {
            // Vector3 targetPos = _target.position;
            // targetPos.y = 0;
            // transform.position = Vector3.SmoothDamp(transform.position, targetPos + _offset, ref _currentVelocity, _smoothing);
            Vector3 targetPosition = _target.position;
            targetPosition.y = 0;
            transform.position = targetPosition + _positionOffset;
            transform.rotation = Quaternion.Euler(_rotationOffset.x, _rotationOffset.y, _rotationOffset.z);
        }

        public void SwitchCamera(ECamera camera)
        {
            switch (camera)
            {
                case ECamera.Battle:
                    // _battleCamera.gameObject.SetActive(true);
                    _battleCamera.enabled = true;
                    GetComponent<Camera>().enabled = false;
                    break;
                case ECamera.World:
                    // _battleCamera.gameObject.SetActive(false);
                    _battleCamera.enabled = false;
                    GetComponent<Camera>().enabled = true;
                    break;
            }
        }

        public void SetEffect(bool enable)
        {
            _godRays.SetActive(enable);
        }

        [Button("Shake")]
        public void Shake(float delay, float strengthMultiplier = 1)
        {
            _battleCamera.DOShakePosition(_duration, _strength * strengthMultiplier, _vibrato);
        }

        public Tweener FocusOn()
        {
            return _battleCamera.DOFieldOfView(55, 0.5f);
        }

        public Tweener FocusOff()
        {
            return _battleCamera.DOFieldOfView(60, 1f);
        }
    }

    public enum ECamera
    {
        World, Battle
    }
}