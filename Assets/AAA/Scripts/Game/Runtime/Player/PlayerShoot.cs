using Game.Input;
using Game.Projectile;
using Unity.Netcode;
using UnityEngine;
using SubsystemManager = Subsystems.Core.SubsystemManager;

namespace Game.Player
{
    public class PlayerShoot : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Collider2D _playerCollider;

        [Header("Settings")]
        [SerializeField]
        private ProjectileType _projectileType;

        [SerializeField]
        private float _fireRate = 1f;


        private float _previousFireTime;

        private InputReader _inputReader;
        private ProjectileLauncher _projectileLauncher;

        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            _inputReader ??= SubsystemManager.TryGetInstance<InputReader>();
            _projectileLauncher ??= SubsystemManager.TryGetInstance<ProjectileLauncher>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
                return;

            _inputReader.OnPlayerPrimaryAttack += HandlePrimaryFire;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
                return;

            _inputReader.OnPlayerPrimaryAttack -= HandlePrimaryFire;
        }

        private void HandlePrimaryFire(bool shouldFire)
        {
            if (!shouldFire)
                return;

            if (Time.time < _previousFireTime + (1 / _fireRate))
                return;


            var position = _spawnPoint.position;
            var direction = _spawnPoint.right * _spawnPoint.parent.localScale.x;

            PrimaryFireHandler_ServerRpc(position, direction);
            _projectileLauncher.SpawnProjectile_Client(position, direction, _projectileType, _playerCollider);

            _previousFireTime = Time.time;
        }

        [ServerRpc]
        private void PrimaryFireHandler_ServerRpc(Vector3 position, Vector3 direction)
        {
            _projectileLauncher
                .SpawnProjectile_Server(position, direction, _projectileType, _playerCollider);

            SpawnClientProjectileResult_ClientRpc(position, direction);
        }

        [ClientRpc]
        private void SpawnClientProjectileResult_ClientRpc(Vector3 position, Vector3 direction)
        {
            if (IsOwner)
                return;

            _projectileLauncher
                .SpawnProjectile_Client(position, direction, _projectileType, _playerCollider);
        }
    }
}