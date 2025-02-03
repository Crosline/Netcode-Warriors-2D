using UnityEngine;

namespace Game.Projectile
{
    public class ProjectileAuthor : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider2D { get; private set; }

        [field: SerializeField]
        public Rigidbody2D Rigidbody2D { get; private set; }
    }
}