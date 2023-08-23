using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;

public class BuffableEntity : NetworkBehaviour
{
    public Unit unit;
    private readonly Dictionary<ScriptableBuff, Buff> _buffs = new ();
    [Networked] public bool IsStunning { get; set; }
    [Networked] public bool IsRetaliateActive { get; set; }

    [Networked] [Capacity(5)] private NetworkArray<BuffNetworkStruct> Buffstatus => default;

    public UnitStatusUI ui;
    public struct BuffNetworkStruct : INetworkStruct
    {
        public NetworkString<_32> Name;
        [Networked] public float RemainingTime { get; set; }
    }

    public override void Spawned()
    {
        unit = GetComponent<Unit>();
        ui = GameObject.Find("UnitAttr6").GetComponent<UnitStatusUI>();
    }

    void FixedUpdate()
    {
        //OPTIONAL, return before updating each buff if game is paused
        //if (Game.isPaused)
        //    return;

        foreach (var buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                ui.deQueue(buff);
                _buffs.Remove(buff.BuffData);
                
            }
        }
    }

    public Buff GetBuff(Type buffType)
    {
        if (!buffType.IsSubclassOf(typeof(Buff)))
        {
            return null;
        }

        foreach (var pair in _buffs)
        {
            if (pair.Value.GetType() == buffType)
            {
                return pair.Value;
            }
        }

        return null;
    }

    public List<Buff> GetBuffs(Type buffType)
    {
        List<Buff> list = new List<Buff>();
        if (!buffType.IsSubclassOf(typeof(Buff)))
        {
            return list;
        }

        foreach (var pair in _buffs)
        {
            if (pair.Value.GetType() == buffType)
            {
                list.Append(pair.Value);
            }
        }

        return list;
    }

    public void AddBuff(ScriptableBuff scriptableBuff)
    {
        if (_buffs.ContainsKey(scriptableBuff))
        {
            _buffs[scriptableBuff].Activate();
        }
        else
        {
            Buff buff = scriptableBuff.InitializeBuff(gameObject);
            _buffs.Add(scriptableBuff, buff);
            buff.Activate();
            ui.addQueue(buff);
        }
    }

    public void ShowBuff() {
        foreach (var buff in _buffs.Values.ToList())
        {
            ui.addQueue(buff);
        }
    }
}