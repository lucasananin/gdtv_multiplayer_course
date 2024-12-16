using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : Singleton<ClientSingleton>
{
    private ClientGameManager _gameManager = null;

    public async Task CreateClient_Async()
    {
        _gameManager = new ClientGameManager();
        await _gameManager.Init_Async();
    }
}
