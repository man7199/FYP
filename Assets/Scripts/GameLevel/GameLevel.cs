using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class GameLevel : NetworkBehaviour
{
    public static GameLevel Instance;
    public GameUI canvas; // please use its function to show ending screen

    [Networked] [UnitySerializeField] public bool isMarrow2Unlock { get; set; }
    [Networked] [UnitySerializeField] public bool isThymusUnlock { get; set; }
    [Networked] [UnitySerializeField] public bool isCellWallUnlock { get; set; }
    [Networked] [UnitySerializeField] public bool isCannonHeroProduced { get; set; }
    [Networked] [UnitySerializeField] public bool isSupportHeroProduced { get; set; }
    [Networked] [UnitySerializeField] public bool isTankHeroProduced { get; set; }
    [Networked] [UnitySerializeField] public int HealthIndex { get; set; } = 50;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Gamelevel place: " + Instance.name + " + " + this.name);
            Debug.LogError("There should be only one Gamelevel.");
            return;
        }

        Instance = this;
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCAddHealthIndex(int num)
    {
        HealthIndex += num;
    }
    public void AddHealthIndex(int num)
    {
        RPCAddHealthIndex(num);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCSetHealthIndex(int num)
    {
        HealthIndex = num;
    }
    public void SetHealthIndex(int num)
    {
        RPCSetHealthIndex(num);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RpcCannon(bool x)
    {
        isCannonHeroProduced = x;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RpcSupport(bool x)
    {
        isSupportHeroProduced = x;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RpcTank(bool x)
    {
        isTankHeroProduced =x;
    }

}