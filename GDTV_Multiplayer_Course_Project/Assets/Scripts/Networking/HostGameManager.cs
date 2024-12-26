using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager
{
    private Allocation _allocation = null;
    private string _joinCode = null;

    private const int MAX_ALLOCATIONS = 20;
    private const string CONNECTION_TYPE = "udp";
    private const string GAME_SCENE_NAME = "Game";

    public async Task StartHost_Async()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(MAX_ALLOCATIONS);
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

        var _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var _relayServerData = new RelayServerData(_allocation, CONNECTION_TYPE);
        _transport.SetRelayServerData(_relayServerData);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE_NAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
