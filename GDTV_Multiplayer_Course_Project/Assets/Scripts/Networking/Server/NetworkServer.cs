using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer
{
    private NetworkManager _networkManager = null;
    private Dictionary<ulong, string> _clientIdToAuth = new();
    private Dictionary<string, UserData> _authIdToUserData = new();

    public NetworkServer(NetworkManager _networkManager)
    {
        this._networkManager = _networkManager;

        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        _networkManager.OnServerStarted += OnNetworkReady;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest _request, NetworkManager.ConnectionApprovalResponse _response)
    {
        var _payload = System.Text.Encoding.UTF8.GetString(_request.Payload);
        var _userData = JsonUtility.FromJson<UserData>(_payload);

        _clientIdToAuth[_request.ClientNetworkId] = _userData.userAuthId;
        _authIdToUserData[_userData.userAuthId] = _userData;

        _response.Approved = true;
        _response.CreatePlayerObject = true;
    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong _clientId)
    {
        if (_clientIdToAuth.TryGetValue(_clientId, out string _authId))
        {
            _clientIdToAuth.Remove(_clientId);
            _authIdToUserData.Remove(_authId);
        }
    }
}
