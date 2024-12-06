using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] InputReader _inputReader = null;
    [SerializeField] Transform _turretTransform = null;

    private void LateUpdate()
    {
        if (!IsOwner) return;
        var _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_inputReader.MousePosition);
        _mouseWorldPosition.z = 0;
        var _aimDirection = (_mouseWorldPosition - _turretTransform.position).normalized;
        _turretTransform.up = _aimDirection;
    }
}
