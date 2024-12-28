using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager
{
    private Allocation _allocation = null;
    private string _joinCode = null;
    private string _lobbyId = null;

    public const int MAX_CONNECTIONS = 20;
    public const string CONNECTION_TYPE = "dtls";
    public const string JOIN_CODE_KEY = "JoinCode";
    public const string GAME_SCENE_NAME = "Game";

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

            var _lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", MAX_CONNECTIONS, _lobbyOptions);
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

        NetworkManager.Singleton.StartHost();
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
}
