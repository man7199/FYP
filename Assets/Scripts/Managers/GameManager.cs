using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class GameManager : NetworkBehaviour
{
    public static event Action<GameManager> OnLobbyDetailsUpdated;

    public static Mission CurrentMission { get; private set; }

    public static GameManager Instance { get; private set; }

    [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public NetworkString<_32> LobbyName { get; set; }
    [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public int MissionId { get; set; }

    private static void OnLobbyDetailsChangedCallback(Changed<GameManager> changed)
    {
        OnLobbyDetailsUpdated?.Invoke(changed.Behaviour);
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            LobbyName = ServerInfo.LobbyName;
            MissionId = ServerInfo.MissionId;
        }
    }

    public static void SetMission(Mission mission)
    {
        CurrentMission = mission;
    }
}
