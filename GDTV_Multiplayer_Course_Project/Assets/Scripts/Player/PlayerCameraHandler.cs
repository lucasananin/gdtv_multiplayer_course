using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera = null;

    const int OWNER_PRIORITY = 20;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _virtualCamera.Priority = OWNER_PRIORITY;
        }
    }
}
