using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private JoinAllocation _joinAllocation = null;

    const string MENU_SCENE_NAME = "menu";

    public async Task<bool> Init_Async()
    {
        await UnityServices.InitializeAsync();

        var _authState = await AuthenticationWrapper.DoAuth();

        if (_authState == AuthState.Authenticated)
        {
            return true;
        }

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    public async Task StartClient_Async(string _joinCode)
    {
        try
        {
            _joinAllocation = await Relay.Instance.JoinAllocationAsync(_joinCode);
        }
        catch (System.Exception _exception)
        {
            throw _exception;
        }

        var _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var _connectionType = HostGameManager.CONNECTION_TYPE;
        var _relayServerData = new RelayServerData(_joinAllocation, _connectionType);
        _transport.SetRelayServerData(_relayServerData);

        NetworkManager.Singleton.StartClient();
    }
}
