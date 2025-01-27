using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenVs : MonoBehaviour
{
    private void Start()
    {
        if (TryGetComponent(out Transform _t))
        {
            _t.SetPositionAndRotation(Vector3.right, Quaternion.identity);
        }
    }
}
