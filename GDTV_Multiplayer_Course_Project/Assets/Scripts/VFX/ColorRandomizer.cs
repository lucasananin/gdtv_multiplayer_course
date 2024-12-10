using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ColorRandomizer : NetworkBehaviour
{
    [SerializeField] SpriteRenderer[] _sprites = null;

    //private Color _color = default;
    private NetworkVariable<Color> _color = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //ChangeColor();
        //ChangeColorClientRpc();

        //if (!IsOwner) return;
        //PickColor();
        //SetSprites();
        //SetSprites_ServerRpc();
        //SetSprites_ClientRpc();

        //Invoke(nameof(Fodase), 20f);

        //if (!IsClient) return;
        //if (IsServer) return;
        if (IsClient)
            _color.OnValueChanged += ChangeColor;

        if (IsServer)
            PickColor();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        //if (!IsClient) return;
        //if (IsServer) return;
        if (IsClient)
            _color.OnValueChanged -= ChangeColor;
    }

    private void ChangeColor(Color previousValue, Color newValue)
    {
        //if (!IsClient) return;
        //if (!IsOwner) return;

        var _count = _sprites.Length;

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = newValue;
        }
    }

    private void Fodase()
    {
        if (IsClient) return;
        if (!IsOwner) return;

        PickColor();
        SetSprites();
        SetSprites_ServerRpc();
        SetSprites_ClientRpc();
    }

    private void PickColor()
    {
        _color.Value = Random.ColorHSV(0, 1, 0.4f, 0.8f, 0.6f, 1, 1, 1);
    }

    private void SetSprites()
    {
        var _count = _sprites.Length;

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _color.Value;
        }
    }

    [ServerRpc]
    private void SetSprites_ServerRpc()
    {
        SetSprites();
    }

    [ClientRpc]
    private void SetSprites_ClientRpc()
    {
        SetSprites();
    }

    //[ClientRpc]
    //private void ChangeColorClientRpc()
    //{
    //    ChangeColor();
    //}

    //private void ChangeColor()
    //{
    //    //if (!IsOwner) return;
    //    var _count = _sprites.Length;
    //    var _randomColor = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);

    //    for (int i = 0; i < _count; i++)
    //    {
    //        _sprites[i].color = _randomColor;
    //    }
    //}
}
