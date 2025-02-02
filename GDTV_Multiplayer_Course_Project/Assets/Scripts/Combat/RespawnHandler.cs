using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] NetworkObject _playerPrefab = null;

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
        Destroy(_tank.gameObject);
        StartCoroutine(RespawnPlayer(_tank.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong _ownerClientId)
    {
        yield return null;

        var _instance = Instantiate(_playerPrefab, SpawnPoint.GetRandomSpawnPosition(), Quaternion.identity);

        _instance.SpawnAsPlayerObject(_ownerClientId);
    }
}
