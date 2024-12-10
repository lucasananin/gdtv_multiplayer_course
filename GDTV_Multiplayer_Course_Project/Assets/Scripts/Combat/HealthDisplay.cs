using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField] Health _health = null;
    [SerializeField] Image _fill = null;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsClient) return;
        _health.CurrentHealth.OnValueChanged += HandleHealth;
        HandleHealth(0, _health.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsClient) return;
        _health.CurrentHealth.OnValueChanged -= HandleHealth;
    }

    private void HandleHealth(int _oldValue, int _newValue)
    {
        var _normalizedValue = _health.GetNormalizedValue();
        _fill.fillAmount = _normalizedValue;
    }
}
