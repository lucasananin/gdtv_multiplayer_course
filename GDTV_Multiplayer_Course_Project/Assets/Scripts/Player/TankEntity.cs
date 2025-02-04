using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankEntity : NetworkBehaviour
{
    [SerializeField] Health _health = null;
    [SerializeField] CoinWallet _coinWallet = null;

    private NetworkVariable<FixedString32Bytes> _playerName = new();

    public static event Action<TankEntity> OnPlayerSpawned = null;
    public static event Action<TankEntity> OnPlayerDespawned = null;

    public Health Health { get => _health; private set => _health = value; }
    public CoinWallet CoinWallet { get => _coinWallet; private set => _coinWallet = value; }
    public NetworkVariable<FixedString32Bytes> PlayerName { get => _playerName; private set => _playerName = value; }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            var _userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            _playerName.Value = _userData.userName;

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
