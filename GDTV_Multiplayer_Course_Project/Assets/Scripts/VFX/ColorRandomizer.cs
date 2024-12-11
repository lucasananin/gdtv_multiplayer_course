using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ColorRandomizer : NetworkBehaviour
{
    [SerializeField] SpriteRenderer[] _sprites = null;

    private NetworkVariable<Color> _color = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            _color.Value = GetRandomColor();

        if (IsClient)
            SetSpritesColor(_color.Value);
    }

    private Color GetRandomColor()
    {
        return Random.ColorHSV(0, 1, 0.4f, 0.8f, 0.6f, 1, 1, 1);
    }

    private void SetSpritesColor(Color _color)
    {
        var _count = _sprites.Length;

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _color;
        }
    }
}
