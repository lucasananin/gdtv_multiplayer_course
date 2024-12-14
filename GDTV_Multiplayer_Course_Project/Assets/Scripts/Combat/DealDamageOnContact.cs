using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] int _damage = 10;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        var _rb = _other.attachedRigidbody;

        if (_rb is not null && _rb.TryGetComponent(out Health _health))
        {
            _health.TakeDamage(_damage);
        }
    }
}
