using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager : IDisposable
{
    private JoinAllocation _joinAllocation = null;
    private NetworkClient _networkClient = null;

    public const string MENU_SCENE_NAME = "menu";

    public async Task<bool> Init_Async()
    {
        await UnityServices.InitializeAsync();

        _networkClient = new NetworkClient(NetworkManager.Singleton);

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

        var _userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, NameSelector.MISSING_NAME),
            userAuthId = AuthenticationService.Instance.PlayerId,
        };
        string _payload = JsonUtility.ToJson(_userData);
        byte[] _payloadBytes = Encoding.UTF8.GetBytes(_payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = _payloadBytes;

        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        _networkClient.Disconnect();
    }

    public void Dispose()
    {
        _networkClient?.Dispose();
    }
}
