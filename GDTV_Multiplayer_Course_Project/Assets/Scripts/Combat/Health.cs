using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    public NetworkVariable<int> CurrentHealth = new();

    public event System.Action<Health> OnDie = null;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        CurrentHealth.Value = MaxHealth;
    }

    [ContextMenu("TakeRandomDamage()")]
    public void TakeRandomDamage()
    {
        var _randomValue = Random.Range(12, 34);
        TakeDamage(_randomValue);
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
        if (IsDead()) return;

        var _newHealth = CurrentHealth.Value + _value;
        CurrentHealth.Value = Mathf.Clamp(_newHealth, 0, MaxHealth);

        if (CurrentHealth.Value <= 0)
        {
            OnDie?.Invoke(this);
        }
    }

    public float GetNormalizedValue()
    {
        return (float)CurrentHealth.Value / MaxHealth;
    }

    public bool IsDead()
    {
        return CurrentHealth.Value <= 0;
    }
}
