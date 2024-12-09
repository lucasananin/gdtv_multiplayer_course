using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ColorRandomizer : NetworkBehaviour
{
    [SerializeField] SpriteRenderer[] _sprites = null;

    private Color _color = default;

    //public override void OnNetworkSpawn()
    //{
    //    base.OnNetworkSpawn();
    //    //ChangeColor();
    //    //ChangeColorClientRpc();

    //    //if (!IsOwner) return;
    //    //PickColor();
    //    //SetSprites();
    //    //SetSprites_ServerRpc();
    //    //SetSprites_ClientRpc();

    //    Invoke(nameof(Fodase), 20f);
    //}

    private void Fodase()
    {
        if (!IsOwner) return;

        PickColor();
        SetSprites();
        SetSprites_ServerRpc();
        SetSprites_ClientRpc();
    }

    private void PickColor()
    {
        _color = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1, 1, 1);
    }

    private void SetSprites()
    {
        var _count = _sprites.Length;

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _color;
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
