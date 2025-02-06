using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] TankEntity _playerPrefab = null;
    [SerializeField, Range(0, 100)] float _keptCoinPercentage = 50;

    protected override void OnNetworkPostSpawn()
    {
        if (!IsServer) return;

        var _tanks = FindObjectsOfType<TankEntity>();
        foreach (var _tank in _tanks)
        {
            TankEntity_OnPlayerSpawned(_tank);
        }

        TankEntity.OnPlayerSpawned += TankEntity_OnPlayerSpawned;
        TankEntity.OnPlayerDespawned += TankEntity_OnPlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;

        TankEntity.OnPlayerSpawned += TankEntity_OnPlayerSpawned;
        TankEntity.OnPlayerDespawned += TankEntity_OnPlayerDespawned;
    }

    private void TankEntity_OnPlayerSpawned(TankEntity _tank)
    {
        _tank.Health.OnDie += (_health) => HandlePlayerDie(_tank);
    }

    private void TankEntity_OnPlayerDespawned(TankEntity _tank)
    {
        _tank.Health.OnDie -= (_health) => HandlePlayerDie(_tank);
    }

    private void HandlePlayerDie(TankEntity _tank)
    {
        var _coinsKept = _tank.CoinWallet.TotalCoins.Value * (_keptCoinPercentage / 100);
        Destroy(_tank.gameObject);
        StartCoroutine(RespawnPlayer(_tank.OwnerClientId, _coinsKept));
    }

    private IEnumerator RespawnPlayer(ulong _ownerClientId, float _coinsKept)
    {
        yield return null;

        var _instance = Instantiate(_playerPrefab, SpawnPoint.GetRandomSpawnPosition(), Quaternion.identity);
        _instance.NetworkObject.SpawnAsPlayerObject(_ownerClientId);
        _instance.CoinWallet.TotalCoins.Value = (int)_coinsKept;
    }
}
