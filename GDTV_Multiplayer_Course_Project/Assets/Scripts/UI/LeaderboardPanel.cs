using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardPanel : NetworkBehaviour
{
    [SerializeField] Transform _content = null;
    [SerializeField] LeaderboardSlot _slotPrefab = null;
    [SerializeField] int _entitiesToDisplay = 8;

    private NetworkList<LeaderboardSlotState> _leaderboardEntities = new();
    private List<LeaderboardSlot> _uiSlots = new();

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            _leaderboardEntities.OnListChanged += _leaderboardEntities_OnListChanged;

            foreach (var _entity in _leaderboardEntities)
            {
                _leaderboardEntities_OnListChanged(
                    new NetworkListEvent<LeaderboardSlotState>
                    {
                        Type = NetworkListEvent<LeaderboardSlotState>.EventType.Add,
                        Value = _entity,
                    });
            }
        }

        if (IsServer)
        {
            var _tanks = FindObjectsByType<TankEntity>(FindObjectsSortMode.None);
            int _count = _tanks.Length;

            for (int i = 0; i < _count; i++)
            {
                HandlePlayerSpawned(_tanks[i]);
            }

            TankEntity.OnPlayerSpawned += HandlePlayerSpawned;
            TankEntity.OnPlayerDespawned += HandlePlayerDespawned;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            _leaderboardEntities.OnListChanged -= _leaderboardEntities_OnListChanged;
        }

        if (IsServer)
        {
            TankEntity.OnPlayerSpawned -= HandlePlayerSpawned;
            TankEntity.OnPlayerDespawned -= HandlePlayerDespawned;
        }
    }

    private void HandlePlayerSpawned(TankEntity _tank)
    {
        var _entity = new LeaderboardSlotState()
        {
            ClientId = _tank.OwnerClientId,
            PlayerName = _tank.PlayerName.Value,
            Coins = 0,
        };

        _leaderboardEntities.Add(_entity);
        _tank.CoinWallet.TotalCoins.OnValueChanged += (_oldCoins, _newCoins) => HandleCoinsChanged(_tank.OwnerClientId, _newCoins);
    }

    private void HandlePlayerDespawned(TankEntity _tank)
    {
        if (IsServer && _tank.OwnerClientId == OwnerClientId) return;
        if (_leaderboardEntities is null) return;

        int _count = _leaderboardEntities.Count;

        for (int i = _count - 1; i >= 0; i--)
        {
            var _entity = _leaderboardEntities[i];

            if (_entity.ClientId == _tank.OwnerClientId)
            {
                _leaderboardEntities.Remove(_entity);
                break;
            }
        }

        _tank.CoinWallet.TotalCoins.OnValueChanged -= (_oldCoins, _newCoins) => HandleCoinsChanged(_tank.OwnerClientId, _newCoins);
    }

    private void HandleCoinsChanged(ulong _clientId, int _newCoinValue)
    {
        int _count = _leaderboardEntities.Count;

        for (int i = 0; i < _count; i++)
        {
            if (_leaderboardEntities[i].ClientId != _clientId) continue;

            _leaderboardEntities[i] = new LeaderboardSlotState
            {
                ClientId = _leaderboardEntities[i].ClientId,
                PlayerName = _leaderboardEntities[i].PlayerName,
                Coins = _newCoinValue,
            };

            return;
        }
    }

    private void _leaderboardEntities_OnListChanged(NetworkListEvent<LeaderboardSlotState> _changeEvent)
    {
        switch (_changeEvent.Type)
        {
            case NetworkListEvent<LeaderboardSlotState>.EventType.Add:
                if (!_uiSlots.Any(x => x.ClientId == _changeEvent.Value.ClientId))
                {
                    var _instance = Instantiate(_slotPrefab, _content);
                    _instance.Init(_changeEvent.Value.ClientId, _changeEvent.Value.PlayerName, _changeEvent.Value.Coins);
                    _uiSlots.Add(_instance);
                }

                break;

            case NetworkListEvent<LeaderboardSlotState>.EventType.Remove:
                var _uiSlotToRemove = _uiSlots.FirstOrDefault(x => x.ClientId == _changeEvent.Value.ClientId);

                if (_uiSlotToRemove is not null)
                {
                    _uiSlotToRemove.transform.SetParent(null);
                    Destroy(_uiSlotToRemove.gameObject);
                    _uiSlots.Remove(_uiSlotToRemove);
                }

                break;

            case NetworkListEvent<LeaderboardSlotState>.EventType.Value:
                var _uiSlotToUpdate = _uiSlots.FirstOrDefault(x => x.ClientId == _changeEvent.Value.ClientId);

                if (_uiSlotToUpdate is not null)
                {
                    _uiSlotToUpdate.UpdateCoins(_changeEvent.Value.Coins);
                }

                break;

            default:
                break;
        }

        _uiSlots.Sort((x, y) => y.Coins.CompareTo(x.Coins));
        int _count = _uiSlots.Count;
        for (int i = 0; i < _count; i++)
        {
            _uiSlots[i].transform.SetSiblingIndex(i);
            _uiSlots[i].UpdateVisuals();
            bool _shouldShow = i <= _entitiesToDisplay;
            _uiSlots[i].gameObject.SetActive(_shouldShow);
        }

        var _myDisplay = _uiSlots.FirstOrDefault(x => x.ClientId == NetworkManager.Singleton.LocalClientId);

        if (_myDisplay != null)
        {
            if (_myDisplay.transform.GetSiblingIndex() >= _entitiesToDisplay)
            {
                _content.GetChild(_entitiesToDisplay - 1).gameObject.SetActive(false);
                _myDisplay.gameObject.SetActive(true);
            }
        }
    }
}
