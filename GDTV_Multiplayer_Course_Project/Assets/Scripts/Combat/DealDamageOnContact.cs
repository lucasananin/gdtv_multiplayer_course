using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] int _damage = 10;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.attachedRigidbody.TryGetComponent(out Health _health))
        {
            _health.TakeDamage(_damage);
        }
    }
}
