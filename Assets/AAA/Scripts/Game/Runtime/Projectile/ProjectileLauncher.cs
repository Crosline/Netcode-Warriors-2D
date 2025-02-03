using Game.Input;
using Unity.Netcode;
using UnityEngine;
using SubsystemManager = Subsystems.Core.SubsystemManager;

namespace Game.Projectile
{
    public class ProjectileLauncher : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Collider2D _playerCollider;

        [SerializeField]
        private ProjectileAuthor _serverPrefab;
        [SerializeField]
        private ProjectileAuthor _clientPrefab;

        [Header("Settings")]
        [SerializeField]
        private float _projectileSpeed = 15f;

        [SerializeField]
        private float _fireRate = 1f;


        private float _previousFireTime;

        private InputReader _inputReader;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
                return;

            // _inputReader = InputReader.Instance;
            _inputReader = SubsystemManager.TryGetInstanceWithoutError<InputReader>();
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

            SpawnClientProjectile(position, direction);
            
            _previousFireTime = Time.time;
        }

        [ServerRpc]
        private void PrimaryFireHandler_ServerRpc(Vector3 spawnPos, Vector3 direction)
        {
            var projectileInstance = Instantiate(
                _serverPrefab,
                spawnPos,
                Quaternion.identity);

            projectileInstance.transform.up = direction;

            Physics2D.IgnoreCollision(_playerCollider, projectileInstance.Collider2D);
            SetProjectileSpeed(projectileInstance.Rigidbody2D);

            SpawnClientProjectileResult_ClientRpc(spawnPos, direction);
        }

        [ClientRpc]
        private void SpawnClientProjectileResult_ClientRpc(Vector3 spawnPos, Vector3 direction)
        {
            if (IsOwner)
                return;

            SpawnClientProjectile(spawnPos, direction);
        }

        private void SpawnClientProjectile(Vector3 spawnPos, Vector3 direction)
        {
            var projectileInstance = Instantiate(
                _clientPrefab,
                spawnPos,
                Quaternion.identity);

            projectileInstance.transform.up = direction;

            Physics2D.IgnoreCollision(_playerCollider, projectileInstance.Collider2D);
            SetProjectileSpeed(projectileInstance.Rigidbody2D);
        }

        private void SetProjectileSpeed(Rigidbody2D rb)
        {
            rb.linearVelocity = rb.transform.up * _projectileSpeed;
        }
    }
}