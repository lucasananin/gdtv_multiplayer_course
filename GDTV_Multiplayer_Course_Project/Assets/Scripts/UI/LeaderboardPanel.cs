using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardPanel : NetworkBehaviour
{
    [SerializeField] Transform _content = null;
    [SerializeField] LeaderboardSlot _slotPrefab = null;

    private NetworkList<LeaderboardSlotState> _leaderboardEntities = new();
}
