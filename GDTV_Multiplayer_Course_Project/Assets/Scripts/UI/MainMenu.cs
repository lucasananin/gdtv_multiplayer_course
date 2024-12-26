using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startHostButton = null;

    private void OnEnable()
    {
        _startHostButton.onClick.AddListener(StartHost);
    }

    private void OnDisable()
    {
        _startHostButton.onClick.RemoveAllListeners();
    }

    private async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHost_Async();
    }
}
