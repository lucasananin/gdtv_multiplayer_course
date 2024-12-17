using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    const string MENU_SCENE_NAME = "menu";

    public async Task<bool> Init_Async()
    {
        await UnityServices.InitializeAsync();

        var _authState = await AuthenticationWrapper.DoAuth();

        if (_authState == AuthState.Authenticated)
        {
            return true;
        }

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}
