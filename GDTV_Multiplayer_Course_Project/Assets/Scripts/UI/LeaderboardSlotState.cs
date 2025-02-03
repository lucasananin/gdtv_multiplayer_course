using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct LeaderboardSlotState : INetworkSerializable , IEquatable<LeaderboardSlotState>
{
    public ulong ClientId;
    public FixedString32Bytes PlayerName;
    public int Coins;

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> _serializer)
    {
        _serializer.SerializeValue(ref ClientId);
        _serializer.SerializeValue(ref PlayerName);
        _serializer.SerializeValue(ref Coins);
    }

    bool IEquatable<LeaderboardSlotState>.Equals(LeaderboardSlotState _other)
    {
        return ClientId == _other.ClientId
            && PlayerName.Equals(_other.PlayerName)
            && Coins == _other.Coins;
    }
}
