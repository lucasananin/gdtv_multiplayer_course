using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float _time = 10f;

    private void Start()
    {
        float _randomValue = Random.Range(_time - 1, _time + 1);
        Destroy(gameObject, _randomValue);
    }
}
