using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer
{
    private NetworkManager _networkManager = null;

    public NetworkServer(NetworkManager _networkManager)
    {
        this._networkManager = _networkManager;

        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest _request, NetworkManager.ConnectionApprovalResponse _response)
    {
        var _payload = System.Text.Encoding.UTF8.GetString(_request.Payload);
        var _userData = JsonUtility.FromJson<UserData>(_payload);

        Debug.Log($"{_userData}");
        Debug.Log($"{_userData.userName}");

        _response.Approved = true;
        _response.CreatePlayerObject = true;
    }
}
