using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] TMP_Text _nameText = null;
    [SerializeField] TMP_Text _playerCountText = null;
    [SerializeField] Button _joinButton = null;

    private LobbiesList _lobbiesList = null;
    private Lobby _lobby = null;

    private void OnEnable()
    {
        _joinButton.onClick.AddListener(Join);
    }

    private void OnDisable()
    {
        _joinButton.onClick.RemoveAllListeners();
    }

    public void Init(LobbiesList _lobbiesList, Lobby _lobby)
    {
        this._lobbiesList = _lobbiesList;
        this._lobby = _lobby;

        _nameText.text = _lobby.Name;
        _playerCountText.text = $"{_lobby.Players.Count}/{_lobby.MaxPlayers}";
    }

    public void Join()
    {
        _ = _lobbiesList.Join_Async(_lobby);
    }
}
