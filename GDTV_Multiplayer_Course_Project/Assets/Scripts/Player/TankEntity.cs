using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankEntity : NetworkBehaviour
{
    [SerializeField] Health _health = null;

    public static event Action<TankEntity> OnPlayerSpawned = null;
    public static event Action<TankEntity> OnPlayerDespawned = null;

    public Health Health { get => _health; private set => _health = value; }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            OnPlayerSpawned?.Invoke(this);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}
