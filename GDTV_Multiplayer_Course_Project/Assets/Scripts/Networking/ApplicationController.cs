using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] ClientSingleton _clientPrefab = null;
    [SerializeField] HostSingleton _hostPrefab = null;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        var _isDedicatedServer = SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;
        await LaunchInMode_Async(_isDedicatedServer);
    }

    private async Task LaunchInMode_Async(bool _isDedicatedServer)
    {
        if (_isDedicatedServer)
        {

        }
        else
        {
            var _clientInstance = Instantiate(_clientPrefab);
            await _clientInstance.CreateClient_Async();

            var _hostInstance = Instantiate(_hostPrefab);
            _hostInstance.CreateHost();

            // Go To Main Menu.
        }
    }
}
