using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer = null;

    protected int _coinValue = 1;
    protected bool _alreadyCollected = false;

    public virtual void SetValue(int _value)
    {
        _coinValue = _value;
    }

    protected virtual void Show(bool _value)
    {
        _spriteRenderer.enabled = _value;
    }

    public abstract int Collect();
}
