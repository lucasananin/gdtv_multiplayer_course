using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : Singleton<ClientSingleton>
{
    public ClientGameManager GameManager { get; private set; } = null;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager?.Dispose();
    }

    public async Task<bool> CreateClient_Async()
    {
        GameManager = new ClientGameManager();
        return await GameManager.Init_Async();
    }
}
