using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : NetworkBehaviour
    {
        private InputReader _inputReader;

        [Header("Components")]
        [SerializeField]
        private Rigidbody2D _rigidbody;

        [Header("Settings")]
        [SerializeField] private float _speed = 5f;


        private Vector2 _movementInput;

        #region Life Cycle

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _rigidbody ??= GetComponent<Rigidbody2D>();
            _inputReader = InputReader.Instance;
            Subscribe();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;

            UnSubscribe();
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


        #region Event Subscriptions

        private void Subscribe()
        {
            _inputReader.OnPlayerMove += OnPlayerMovement;
        }

        private void UnSubscribe()
        {
            _inputReader.OnPlayerMove -= OnPlayerMovement;
        }

        #endregion
    }
}