using UnityEngine;

namespace Game.Object
{
    public class LifetimeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _lifetime = 3f;
        private void Awake()
        {
            Destroy(gameObject, _lifetime);
        }
    }
}