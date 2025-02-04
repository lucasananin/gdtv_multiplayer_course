using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LeaderboardSlot : MonoBehaviour
{
    [SerializeField] TMP_Text _displayText = null;

    private ulong _clientId = default;
    private FixedString32Bytes _playerName = default;
    private int _coins = default;

    public ulong ClientId { get => _clientId; }
    public int Coins { get => _coins; }

    public void Init(ulong _clientId, FixedString32Bytes _playerName, int _coins)
    {
        this._clientId = _clientId;
        this._playerName = _playerName;

        UpdateCoins(_coins);
    }

    public void UpdateCoins(int _value)
    {
        _coins = _value;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        _displayText.text = $"{1}. {_playerName} ({_coins})";
    }
}
