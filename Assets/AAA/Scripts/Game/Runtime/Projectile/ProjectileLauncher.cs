using System;
using JetBrains.Annotations;
using Subsystems;
using UnityEngine;
using UnityEngine.Scripting;

namespace Game.Projectile
{
    [Preserve]
    [UsedImplicitly]
    public class ProjectileLauncher : GameSubsystem<ProjectileLauncher>
    {
        private ProjectileSettings _settings;

        public override void OnAwake()
        {
            _settings = Resources.Load<ProjectileSettings>(nameof(ProjectileSettings));
        }

        public void SpawnProjectile_Client(Vector3 position, Vector3 direction, ProjectileType type, Collider2D ignoreCollider)
        {
            if (!_settings.ProjectileAuthors.TryGetValue(type, out var authorPair))
                throw new Exception($"Projectile type {type} not found in settings");

            SpawnProjectile(position, direction, authorPair.Client, ignoreCollider);
        }

        public void SpawnProjectile_Server(Vector3 position, Vector3 direction, ProjectileType type, Collider2D ignoreCollider)
        {
            if (!_settings.ProjectileAuthors.TryGetValue(type, out var authorPair))
                throw new Exception($"Projectile type {type} not found in settings");

            SpawnProjectile(position, direction, authorPair.Server, ignoreCollider);
        }

        private static void SpawnProjectile(Vector3 position, Vector3 direction, ProjectileAuthor prefab, Collider2D ignoreCollider)
        {
            var projectileInstance = UnityEngine.Object.Instantiate(
                prefab,
                position,
                Quaternion.identity);

            projectileInstance.transform.up = direction;

            Physics2D.IgnoreCollision(ignoreCollider, projectileInstance.Collider2D);
            SetProjectileSpeed(projectileInstance);
        }

        private static void SetProjectileSpeed(ProjectileAuthor projectile)
        {
            projectile.Rigidbody2D.linearVelocity = projectile.Rigidbody2D.transform.up * projectile.Speed;
        }
    }
}