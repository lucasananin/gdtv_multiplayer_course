using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] RespawningCoin _coinPrefab = null;
    [SerializeField] int _maxCoins = 50;
    [SerializeField] int _coinValue = 1;
    [SerializeField] Vector2 _xSpawnRange = default;
    [SerializeField] Vector2 _ySpawnRange = default;
    [SerializeField] LayerMask _layerMask = default;

    private Collider2D[] _results = new Collider2D[9];

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        for (int i = 0; i < _maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        var _instance = Instantiate(_coinPrefab, GetSpawnPoint(), Quaternion.identity, transform);
        _instance.SetValue(_coinValue);
        _instance.GetComponent<NetworkObject>().Spawn();

        _instance.OnCollected += RepositionCollectedCoin;
    }

    private void RepositionCollectedCoin(RespawningCoin _coin)
    {
        _coin.transform.position = GetSpawnPoint();
        _coin.ResetValues();
    }

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            float _x = Random.Range(_xSpawnRange.x, _ySpawnRange.y);
            float _y = Random.Range(_ySpawnRange.x, _ySpawnRange.y);
            var _spawnPoint = new Vector2(_x, _y);

            var _hits = Physics2D.OverlapCircleNonAlloc(_spawnPoint, 1f, _results, _layerMask);

            if (_hits == 0)
            {
                return _spawnPoint;
            }
        }
    }
}
