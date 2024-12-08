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
        //ChangeColor();
        //ChangeColorClientRpc();
    }

    //private void Awake()
    //{
    //    ChangeColor();
    //}

    [ClientRpc]
    private void ChangeColorClientRpc()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        //if (!IsOwner) return;
        var _count = _sprites.Length;
        var _randomColor = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _randomColor;
        }
    }
}
