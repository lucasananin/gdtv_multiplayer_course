using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] InputReader _inputReader = null;

    private void OnEnable()
    {
        _inputReader.OnMoveEvent += _inputReader_OnMoveEvent;
    }

    private void OnDisable()
    {
        _inputReader.OnMoveEvent -= _inputReader_OnMoveEvent;
    }

    private void _inputReader_OnMoveEvent(Vector2 _obj)
    {
        Debug.Log($"// onMove: {_obj}", this);
    }
}
