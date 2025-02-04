using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardSlot : MonoBehaviour
{
    [SerializeField] TMP_Text _displayText = null;
    [SerializeField] Color _myColor = default;

    private ulong _clientId = default;
    private FixedString32Bytes _playerName = default;
    private int _coins = default;

    public ulong ClientId { get => _clientId; }
    public int Coins { get => _coins; }

    public void Init(ulong _clientId, FixedString32Bytes _playerName, int _coins)
    {
        this._clientId = _clientId;
        this._playerName = _playerName;

        if (_clientId == NetworkManager.Singleton.LocalClientId)
        {
            _displayText.color = _myColor;
        }

        UpdateCoins(_coins);
    }

    public void UpdateCoins(int _value)
    {
        _coins = _value;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var _index = transform.GetSiblingIndex() + 1;
        _displayText.text = $"{_index}. {_playerName} ({_coins})";
    }
}
