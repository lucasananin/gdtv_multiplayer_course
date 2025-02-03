using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameDisplay /*: NetworkBehaviour*/ : MonoBehaviour
{
    [SerializeField] TankEntity _tank = null;
    [SerializeField] TMP_Text _text = null;

    //private NetworkVariable<FixedString32Bytes> PlayerName = new();

    private void OnEnable()
    {
        _tank.PlayerName.OnValueChanged += UpdateVisuals;
    }

    private void OnDisable()
    {
        _tank.PlayerName.OnValueChanged += UpdateVisuals;
    }

    private void Start()
    {
        UpdateVisuals(string.Empty, _tank.PlayerName.Value);
    }

    //public override void OnNetworkSpawn()
    //{
    //    //if (IsServer)
    //    //{
    //    //    var _userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
    //    //    PlayerName.Value = _userData.userName;
    //    //}

    //    if (IsClient)
    //    {
    //        UpdateVisuals(string.Empty, _tank.PlayerName.Value);
    //        _tank.PlayerName.OnValueChanged += UpdateVisuals;
    //    }
    //}

    private void UpdateVisuals(FixedString32Bytes _previousValue, FixedString32Bytes _newValue)
    {
        _text.text = _newValue.ToString();
    }
}
