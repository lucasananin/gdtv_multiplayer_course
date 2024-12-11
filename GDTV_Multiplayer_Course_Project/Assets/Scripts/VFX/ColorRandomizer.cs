using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ColorRandomizer : NetworkBehaviour
{
    [SerializeField] SpriteRenderer[] _sprites = null;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SendColorRpc();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    private void SendColorRpc()
    {
        if (!IsClient) return;
        if (!IsOwner) return;

        var _color = GetRandomColor();
        SetSprites(_color);
        SetSprites_ServerRpc(_color);
        SetSprites_ClientRpc(_color);
    }

    private Color GetRandomColor()
    {
        return Random.ColorHSV(0, 1, 0.4f, 0.8f, 0.6f, 1, 1, 1);
    }

    private void SetSprites(Color _color)
    {
        var _count = _sprites.Length;

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _color;
        }
    }

    [ServerRpc]
    private void SetSprites_ServerRpc(Color _color)
    {
        SetSprites(_color);
    }

    [ClientRpc]
    private void SetSprites_ClientRpc(Color _color)
    {
        SetSprites(_color);
    }
}
