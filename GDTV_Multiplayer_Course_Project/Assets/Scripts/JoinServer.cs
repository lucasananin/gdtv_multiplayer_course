using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinServer : MonoBehaviour
{
//#if UNITY_EDITOR
//    private void Start()
//    {
//        StartHost();
//    }
//#endif

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
    }
}
