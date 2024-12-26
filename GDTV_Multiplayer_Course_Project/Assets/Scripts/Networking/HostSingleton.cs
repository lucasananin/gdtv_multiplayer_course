using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : Singleton<HostSingleton>
{
    public HostGameManager GameManager { get; private set; } = null;

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
