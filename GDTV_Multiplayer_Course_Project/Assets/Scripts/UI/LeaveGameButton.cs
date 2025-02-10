using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LeaveGameButton : MonoBehaviour
{
    [SerializeField] Button _button = null;

    private void OnEnable()
    {
        _button.onClick.AddListener(Leave);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }

        ClientSingleton.Instance.GameManager.Disconnect();
    }
}
