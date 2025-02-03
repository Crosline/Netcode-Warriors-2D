using Game.Input;
using Unity.Netcode;
using UnityEngine;
using SubsystemManager = Subsystems.Core.SubsystemManager;

namespace Game.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private Rigidbody2D _rigidbody;

        [Header("Settings")]
        [SerializeField] private float _speed = 5f;

        private InputReader _inputReader;

        private Vector2 _movementInput;

        #region Life Cycle

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _rigidbody ??= GetComponent<Rigidbody2D>();
            // _inputReader = InputReader.Instance;
            _inputReader = SubsystemManager.TryGetInstanceWithoutError<InputReader>();
            _inputReader.OnPlayerMove += OnPlayerMovement;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;
            _inputReader.OnPlayerMove -= OnPlayerMovement;
        }

        #endregion

        private void OnPlayerMovement(Vector2 playerMovementInput)
        {
            _movementInput = playerMovementInput;
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            _rigidbody.linearVelocity = _movementInput * _speed;
        }
    }
}