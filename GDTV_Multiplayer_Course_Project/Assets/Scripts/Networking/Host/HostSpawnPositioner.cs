using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostSpawnPositioner : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            Fodase_ClientRpc();
        }
    }

    [ClientRpc]
    void Fodase_ClientRpc()
    {
        StartCoroutine(Fodase_routine());
    }

    IEnumerator Fodase_routine()
    {
        yield return new WaitForSeconds(1f);
        transform.position = SpawnPoint.GetRandomSpawnPosition();
    }
}
