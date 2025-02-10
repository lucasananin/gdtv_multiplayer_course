using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager = null;
    private Dictionary<ulong, string> _clientIdToAuth = new();
    private Dictionary<string, UserData> _authIdToUserData = new();

    public Action<string> OnClientLeft = null;

    public NetworkServer(NetworkManager _networkManager)
    {
        this._networkManager = _networkManager;

        this._networkManager.ConnectionApprovalCallback += ApprovalCheck;
        this._networkManager.OnServerStarted += OnNetworkReady;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest _request, NetworkManager.ConnectionApprovalResponse _response)
    {
        var _payload = System.Text.Encoding.UTF8.GetString(_request.Payload);
        var _userData = JsonUtility.FromJson<UserData>(_payload);

        _clientIdToAuth[_request.ClientNetworkId] = _userData.userAuthId;
        _authIdToUserData[_userData.userAuthId] = _userData;

        _response.Approved = true;
        _response.Position = SpawnPoint.GetRandomSpawnPosition();
        _response.Rotation = Quaternion.identity;
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
            OnClientLeft?.Invoke(_authId);
        }
    }

    public UserData GetUserDataByClientId(ulong _clientId)
    {
        if (_clientIdToAuth.TryGetValue(_clientId, out string _authIdValue))
        {
            if (_authIdToUserData.TryGetValue(_authIdValue, out UserData _userDataValue))
            {
                return _userDataValue;
            }
        }

        return null;
    }

    public void Dispose()
    {
        if (_networkManager is null) return;

        _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        _networkManager.OnServerStarted -= OnNetworkReady;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        if (_networkManager.IsListening)
        {
            _networkManager.Shutdown();
        }
    }
}
