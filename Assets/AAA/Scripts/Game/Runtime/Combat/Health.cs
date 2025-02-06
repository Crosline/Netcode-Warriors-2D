using System;
using Unity.Netcode;
using UnityEngine;

namespace Game.Combat
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; } = 100;

        [HideInInspector]
        public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(100);
        
        public bool IsDead => CurrentHealth.Value <= 0;
        
        public Action<Health> OnDeath;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            
            CurrentHealth.Value = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            ModifyHealth(-damage);
        }

        public void RestoreHealth(int heal)
        {
            ModifyHealth(heal);
        }

        private void ModifyHealth(int value)
        {
            if (IsDead) return;

            var newHealth = CurrentHealth.Value + value;
            CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
            
            if (IsDead)
                OnDeath?.Invoke(this);
        }

    }
}