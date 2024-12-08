using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [SerializeField] InputReader _inputReader = null;
    [SerializeField] GameObject _serverProjectilePrefab = null;
    [SerializeField] GameObject _clientProjectilePrefab = null;
    [SerializeField] Transform _muzzle = null;
    [Space]
    [SerializeField] float _projectileSpeed = 20f;

    public event System.Action OnShoot = null;

    //private bool _shouldFire = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        _inputReader.OnPrimaryFireEvent += _inputReader_OnPrimaryFireEvent;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner) return;
        _inputReader.OnPrimaryFireEvent -= _inputReader_OnPrimaryFireEvent;
    }

    //private void Update()
    //{
    //    if (!IsOwner) return;
    //    if (!_shouldFire) return;

    //    PrimaryFireServerRpc(_muzzle.position, _muzzle.up);
    //    SpawnDummyProjectile(_muzzle.position, _muzzle.up);
    //}

    private void _inputReader_OnPrimaryFireEvent(bool _value)
    {
        //_shouldFire = _value;

        if (_value)
        {
            PrimaryFireServerRpc(_muzzle.position, _muzzle.up);
            SpawnDummyProjectile(_muzzle.position, _muzzle.up);
            OnShoot?.Invoke();
        }
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 _position, Vector3 _direction)
    {
        var _instance = Instantiate(_serverProjectilePrefab, _position, Quaternion.identity);
        _instance.transform.up = _direction;

        if (_instance.TryGetComponent(out Rigidbody2D _rb))
        {
            _rb.velocity = _direction * _projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(_position, _direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 _position, Vector3 _direction)
    {
        if (IsOwner) return;
        SpawnDummyProjectile(_position, _direction);
    }

    private void SpawnDummyProjectile(Vector3 _position, Vector3 _direction)
    {
        var _instance = Instantiate(_clientProjectilePrefab, _position, Quaternion.identity);
        _instance.transform.up = _direction;

        if (_instance.TryGetComponent(out Rigidbody2D _rb))
        {
            _rb.velocity = _direction * _projectileSpeed;
        }
    }
}
