using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [SerializeField] NetworkVariable<int> _currentHealth = new();

    private bool _isDead = false;

    public event System.Action<Health> OnDie = null;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        _currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int _value)
    {
        ModifyHealth(-_value);
    }

    public void RestoreHealth(int _value)
    {
        ModifyHealth(_value);
    }

    public void ModifyHealth(int _value)
    {
        if (_isDead) return;

        _currentHealth.Value += _value;
        
        if (_currentHealth.Value <= 0)
        {
            _currentHealth.Value = 0;
            _isDead = true;
            OnDie?.Invoke(this);
        }
    }
}
