using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameDisplay : NetworkBehaviour
{
    [SerializeField] TMP_Text _text = null;

    private NetworkVariable<FixedString32Bytes> PlayerName = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            var _userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            PlayerName.Value = _userData.userName;
        }

        if (IsClient)
        {
            UpdateVisuals(string.Empty, PlayerName.Value);
        }
    }

    private void UpdateVisuals(FixedString32Bytes _previousValue, FixedString32Bytes _newValue)
    {
        _text.text = _newValue.ToString();
    }
}
