using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class MineRobot : Robot
{
    public GameObject minePrefab;
    public GameObject firepoint_1;

    private int availableMines = 50;
    private int MaxMines = 100;
    private float mineRechargeTime = 6f;
    private float timer;

    private float mineRechargeTime1 = 1f;

    Coroutine mineProgress;
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MoveOnlySubtree(this); 
    }
    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Robot/minerobot");
        description[3] = "Set Mines";
        sprite[3] = Resources.Load<Sprite>("Sprites/mine");

    }
    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer >= mineRechargeTime) 
        {
            if (availableMines < MaxMines)
            {
                availableMines += 1;
                timer = 0;
            } 

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Skill1();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Skill2();
        }


    }

    public override void Skill4()
    {
        if (mineProgress == null)
        {
            mineProgress = StartCoroutine(setMines());
            description[3] = "Stop Mines";
            sprite[3] = Resources.Load<Sprite>("Arts/UI/stopui");
            if (UI != null)
                UI.Refresh(this);
        }
        else
        {
            StopCoroutine(mineProgress);
            description[3] = "Set Mines";
            sprite[3] = Resources.Load<Sprite>("Sprites/mine");
            if (UI != null) 
                UI.Refresh(this);
            mineProgress = null;
        }
    }

  
    IEnumerator setMines()
    {
        while (true)
        {
            if (availableMines > 0)
            {
                var affectedObjects = Physics.OverlapSphere(transform.position, 2, LayerMask.GetMask("Mine"));
                if (affectedObjects.Length <= 0)
                {
                    availableMines -= 1;

                    RPCSpawnMine();
                    RPCSound();
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    //Debug.Log(affectedObjects.ToString());
                }
            }
            yield return null;
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCSpawnMine()
    {
        Runner.Spawn(
            minePrefab, firepoint_1.transform.position, transform.rotation);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPCSound()
    {
        AudioManager.Play("setMine", AudioManager.MixerTarget.SFX, transform.position);
    }


    private string UnitInfo = "Hello! I'm a Mine Robot, which is a type of robot. I'm a friendly unit who puts explosive mines onto the field. The explosive mines cause area damage to all in-range units including friendly units when being trigger. The explosive mines would be triggered by all types of non-robot units, including the friendly cells, so be careful to use them, and remind your teammate about the mine!";
    public override string getInfo()
    {
        return UnitInfo;
    }

}
