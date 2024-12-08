using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _sprites = null;

    private void Awake()
    {
        var _count = _sprites.Length;
        var _randomColor = Random.ColorHSV();

        for (int i = 0; i < _count; i++)
        {
            _sprites[i].color = _randomColor;
        }
    }
}
