using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient
{
    private NetworkManager _networkManager = null;

    public NetworkClient(NetworkManager _networkManager)
    {
        this._networkManager = _networkManager;

        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong _clientId)
    {
        if (_clientId != 0 && _clientId != _networkManager.LocalClientId) return;

        var _menuSceneName = ClientGameManager.MENU_SCENE_NAME;

        if (SceneManager.GetActiveScene().name != _menuSceneName)
        {
            SceneManager.LoadScene(_menuSceneName);
        }

        if (_networkManager.IsConnectedClient)
        {
            _networkManager.Shutdown();
        }
    }
}
