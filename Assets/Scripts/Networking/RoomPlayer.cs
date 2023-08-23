using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        GameReady
    }

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;

    public static RoomPlayer Local;

    [Networked(OnChanged = nameof(OnStateChanged))] public NetworkBool IsReady { get; set; }
    [Networked(OnChanged = nameof(OnStateChanged))] public NetworkString<_32> Username { get; set; }
    //[Networked] public NetworkBool HasFinished { get; set; }
    [Networked] public EGameState GameState { get; set; }

    public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Local = this;

            PlayerChanged?.Invoke(this);
            RPC_SetPlayer(ClientInfo.Username);
        }

        Players.Add(this);
        PlayerJoined?.Invoke(this);

        DontDestroyOnLoad(gameObject);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, InvokeResim = true)]
    private void RPC_SetPlayer(NetworkString<_32> username)
    {
        Username = username;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ChangeReadyState(NetworkBool state)
    {
        Debug.Log($"Setting {Object.Name} ready state to {state}");
        IsReady = state;
    }

    private void OnDisable()
    {
        PlayerLeft?.Invoke(this);
        Players.Remove(this);
    }

    private static void OnStateChanged(Changed<RoomPlayer> changed) => PlayerChanged?.Invoke(changed.Behaviour);

    public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
    {
        var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
        if (roomPlayer != null)
        {
            Players.Remove(roomPlayer);
            runner.Despawn(roomPlayer.Object);
        }
    }
}
