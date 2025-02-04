using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
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
