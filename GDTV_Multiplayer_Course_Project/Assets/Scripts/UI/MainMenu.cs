using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startHostButton = null;
    [SerializeField] Button _clientButton = null;
    [SerializeField] TMP_InputField _joinInputField = null;

    private void OnEnable()
    {
        _startHostButton.onClick.AddListener(StartHost);
        _clientButton.onClick.AddListener(StartClient);
    }

    private void OnDisable()
    {
        _startHostButton.onClick.RemoveAllListeners();
        _clientButton.onClick.RemoveAllListeners();
    }

    private async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHost_Async();
    }

    private async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClient_Async(_joinInputField.text);
    }
}
