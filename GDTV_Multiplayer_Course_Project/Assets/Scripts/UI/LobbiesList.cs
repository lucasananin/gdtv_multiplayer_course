using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbiesList : MonoBehaviour
{
    [SerializeField] Transform _contentTransform = null;
    [SerializeField] LobbyItem _lobbyPrefab = null;

    private Lobby _joiningLobby = null;
    private bool _isJoining = false;
    private bool _isRefreshing = false;

    private void OnEnable()
    {
        RefreshList();
    }

    public async void RefreshList()
    {
        if (_isRefreshing) return;

        _isRefreshing = true;

        try
        {
            var _options = new QueryLobbiesOptions();
            _options.Count = 25;

            _options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0")
            };

            var _lobbies = await Lobbies.Instance.QueryLobbiesAsync(_options);

            foreach (Transform _child in _contentTransform)
            {
                Destroy(_child.gameObject);
            }

            foreach (var _lobby in _lobbies.Results)
            {
                var _lobbyItem = Instantiate(_lobbyPrefab, _contentTransform);
                _lobbyItem.Init(this, _lobby);
            }
        }
        catch (LobbyServiceException _exception)
        {
            throw _exception;
        }

        _isRefreshing = false;
    }

    public async Task Join_Async(Lobby _lobbyValue)
    {
        if (_isJoining) return;

        _isJoining = true;

        try
        {
            _joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(_lobbyValue.Id);
            var _key = HostGameManager.JOIN_CODE_KEY;
            var _joinCode = _joiningLobby.Data[_key].Value;

            await ClientSingleton.Instance.GameManager.StartClient_Async(_joinCode);
        }
        catch (LobbyServiceException _exception)
        {
            throw _exception;
        }

        _isJoining = false;
    }
}
