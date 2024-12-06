using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] InputReader _inputReader = null;
    [SerializeField] Transform _bodyTransform = null;
    [SerializeField] Rigidbody2D _rb = null;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _turnSpeed = 30f;

    private Vector2 _moveDirection = default;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //if (!IsOwner) return;
        _inputReader.OnMoveEvent += _inputReader_OnMoveEvent;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        //if (!IsOwner) return;
        _inputReader.OnMoveEvent -= _inputReader_OnMoveEvent;
    }

    private void Update()
    {
        //if (!IsOwner) return;
        var _zRotation = -_moveDirection.x * _turnSpeed * Time.deltaTime;
        _bodyTransform.Rotate(0f, 0f, _zRotation);
    }

    private void FixedUpdate()
    {
        //if (!IsOwner) return;
        var _velocity = _bodyTransform.up * _moveDirection.y * _moveSpeed;
        _rb.velocity = _velocity;
    }

    private void _inputReader_OnMoveEvent(Vector2 _value)
    {
        _moveDirection = _value;
    }
}
