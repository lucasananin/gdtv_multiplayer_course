using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> _totalCoins = new();

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out Coin _coin))
        {
            var _coinValue = _coin.Collect();

            if (IsServer)
            {
                _totalCoins.Value += _coinValue;
            }
        }
    }

    public void SpendCoins(int _value)
    {
        var _coins = _totalCoins.Value - _value;
        _totalCoins.Value = Mathf.Clamp(_coins, 0, 123456);
    }

    public bool HasEnoughCoins(int _value)
    {
        return _totalCoins.Value >= _value;
    }
}
