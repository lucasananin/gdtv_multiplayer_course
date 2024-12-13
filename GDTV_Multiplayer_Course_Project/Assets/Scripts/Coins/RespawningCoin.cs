using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    private Vector3 _previousPosition = default;

    public event System.Action<RespawningCoin> OnCollected = null;

    private void Update()
    {
        if (_previousPosition != transform.position)
        {
            Show(true);
        }

        _previousPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        else
        {
            if (_alreadyCollected) return 0;

            _alreadyCollected = true;
            OnCollected?.Invoke(this);

            return _coinValue;
        }
    }

    internal void ResetValues()
    {
        _alreadyCollected = false;
        //Show(true);
    }
}
