using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;

    private void OnDestroy()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
