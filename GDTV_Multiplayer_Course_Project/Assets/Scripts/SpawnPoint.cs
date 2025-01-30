using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static List<SpawnPoint> _spawnPoints = new();

    private void OnEnable()
    {
        _spawnPoints.Add(this);
    }

    private void OnDisable()
    {
        _spawnPoints.Remove(this);
    }

    public static Vector3 GetRandomSpawnPosition()
    {
        if (_spawnPoints.Count <= 0)
        {
            return Vector3.zero;
        }

        int _randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[_randomIndex].transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
