using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class HostGameManager : IDisposable
{
    private Allocation _allocation = null;
    private NetworkServer _networkServer = null;
    private string _joinCode = null;
    private string _lobbyId = null;

    public const int MAX_CONNECTIONS = 20;
    public const string CONNECTION_TYPE = "dtls";
    public const string JOIN_CODE_KEY = "JoinCode";
    public const string GAME_SCENE_NAME = "Game";

    public NetworkServer NetworkServer { get => _networkServer; private set => _networkServer = value; }

    public async Task StartHost_Async()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
        }
        catch (System.Exception _exception)
        {
            throw _exception;
        }

        try
        {
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log($"// Join Code: {_joinCode}");
        }
        catch (System.Exception _exception)
        {
            throw _exception;
        }

        try
        {
            var _lobbyOptions = new CreateLobbyOptions();
            _lobbyOptions.IsPrivate = false;
            _lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    JOIN_CODE_KEY, new DataObject
                    (
                    visibility: DataObject.VisibilityOptions.Member,
                    value : _joinCode
                    )
                }
            };

            var _lobbyName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, NameSelector.MISSING_NAME);
            var _lobby = await Lobbies.Instance.CreateLobbyAsync($"{_lobbyName}'s Lobby", MAX_CONNECTIONS, _lobbyOptions);
            _lobbyId = _lobby.Id;

            HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15));
        }
        catch (LobbyServiceException _lobbyException)
        {
            throw _lobbyException;
        }

        var _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var _relayServerData = new RelayServerData(_allocation, CONNECTION_TYPE);
        _transport.SetRelayServerData(_relayServerData);

        _networkServer = new NetworkServer(NetworkManager.Singleton);

        var _userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, NameSelector.MISSING_NAME),
            userAuthId = AuthenticationService.Instance.PlayerId,
        };
        string _payload = JsonUtility.ToJson(_userData);
        byte[] _payloadBytes = Encoding.UTF8.GetBytes(_payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = _payloadBytes;

        NetworkManager.Singleton.StartHost();
        NetworkServer.OnClientLeft += HandleClientLeft;
        NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE_NAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private IEnumerator HeartBeatLobby(float _waitTimeInSeconds)
    {
        var _waitTime = new WaitForSecondsRealtime(_waitTimeInSeconds);

        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);
            yield return _waitTime;
        }
    }

    public void Dispose()
    {
        Shutdown();
    }

    public async void Shutdown()
    {
        HostSingleton.Instance.StopCoroutine(nameof(HeartBeatLobby));

        if (!string.IsNullOrEmpty(_lobbyId))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
            }
            catch (LobbyServiceException _exception)
            {
                throw _exception;
            }

            _lobbyId = string.Empty;
        }

        NetworkServer.OnClientLeft -= HandleClientLeft;
        _networkServer?.Dispose();
    }

    private async void HandleClientLeft(string _authId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_lobbyId, _authId);
        }
        catch (LobbyServiceException _lobbyException)
        {
            throw _lobbyException;
        }
    }
}
