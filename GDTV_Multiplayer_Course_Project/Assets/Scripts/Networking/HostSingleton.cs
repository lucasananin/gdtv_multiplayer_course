using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : Singleton<HostSingleton>
{
    private HostGameManager _gameManager = null;

    public void CreateHost()
    {
        _gameManager = new HostGameManager();
    }
}
