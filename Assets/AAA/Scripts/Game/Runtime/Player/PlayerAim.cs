using Game.Input;
using Unity.Netcode;
using UnityEngine;
using SubsystemManager = Subsystems.Core.SubsystemManager;

namespace Game.Player
{
    public class PlayerAim : NetworkBehaviour
    {
        [Header("Components")] [SerializeField]
        private Transform _weaponHolderTransform;

        private InputReader _inputReader;

        private Vector2 _aimInput;

        private Camera _camera;

        #region Life Cycle

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            // _inputReader = InputReader.Instance;
            _inputReader = SubsystemManager.TryGetInstanceWithoutError<InputReader>();
            _inputReader.OnPlayerAim += OnAim;

            _camera = Camera.main;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;

            _inputReader.OnPlayerAim -= OnAim;
        }

        #endregion

        private void Update()
        {
            if (!IsOwner) return;
            
            
            var aimInput = GetAimDirection();

            var isLeft = aimInput.x < 0;
            
            var angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
            _weaponHolderTransform.localRotation = Quaternion.Euler(0f, 0f, isLeft ? angle - 180f : angle);
            _weaponHolderTransform.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        }


        private Vector2 GetAimDirection()
        {
            var direction = _camera.ScreenToWorldPoint(_aimInput) - transform.position;
            direction.z = 0f;

            return direction.normalized;
        }


        private void OnAim(Vector2 playerAimInput)
        {
            _aimInput = playerAimInput;
            
            
            if (_inputReader.InputType == InputType.Gamepad)
                _aimInput += transform.position.ToVector2();
        }
    }
    

    public static class Vector2Extensions
    {
        public static Vector2 WithX(this Vector2 v, float x) => new Vector2(x, v.y);
        public static Vector2 WithY(this Vector2 v, float y) => new Vector2(v.x, y);
        
        public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);
            
    }
}