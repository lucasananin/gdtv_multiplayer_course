using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    [SerializeField] Health _health = null;
    [SerializeField] BountyCoin _bountyCoinPrefab = null;
    [SerializeField] int _maxCountyCoinsDrop = 10;
    [SerializeField] int _minCountyCoinsDrop = 5;
    [SerializeField] LayerMask _layerMask = default;

    private Collider2D[] _results = new Collider2D[9];
    private float _coinRadius = 2f;

    public NetworkVariable<int> TotalCoins = new();

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out Coin _coin))
        {
            var _coinValue = _coin.Collect();

            if (IsServer)
            {
                TotalCoins.Value += _coinValue;
            }
        }
    }

    public void SpendCoins(int _value)
    {
        var _coins = TotalCoins.Value - _value;
        TotalCoins.Value = Mathf.Clamp(_coins, 0, 123456);
    }

    public bool HasEnoughCoins(int _value)
    {
        return TotalCoins.Value >= _value;
    }
}
