using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Support : Hero
{
    [SerializeField] private float buffRange = 30;
    [SerializeField] private ScriptableHealBuff scriptableHealBuff;
    [SerializeField] private ScriptableATKUpBuff scriptableAtkUpBuff;
    [SerializeField] private ScriptablSpeedUpBuff scriptablSpeedUpBuff;
    private int _unitLayer;


    protected override void Awake()
    {
        base.Awake();
        _unitLayer = Global.UNIT_MASK;
        CD4 = 3f;
        CD5 = 3f;
        CD6 = 3f;

        // Set UI
        sprite[3] = Resources.Load<Sprite>("Arts/UI/Icon/red_17");
        sprite[4] = Resources.Load<Sprite>("Arts/UI/Icon/SGI_126");
        sprite[5] = Resources.Load<Sprite>("Arts/UI/Icon/addon_09");
        AOEmaxRange = buffRange;
        description[3] = "Healing";
        description[4] = "Power Up";
        description[5] = "Speed Up";

        icon = Resources.Load<Sprite>("Icons/Robot/utility_robot");
    }

    // Update is called once per frame
    protected override Node SetupBehaviorTree()
    {
        return Subtree.BasicSubtree(this);
    }


    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCHeal()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();

            if (unit.GetType().IsSubclassOf(typeof(Cell)) || unit.GetType().IsSubclassOf(typeof(Robot)))
            {
                unit.buffableEntity.AddBuff(scriptableHealBuff);
            }
        }

        AudioManager.Play("heal", AudioManager.MixerTarget.SFX, transform.position);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCATKUp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();

            if (unit.GetType().IsSubclassOf(typeof(Cell)) || unit.GetType().IsSubclassOf(typeof(Robot)))
            {
                unit.buffableEntity.AddBuff(scriptableAtkUpBuff);
            }
        }

        AudioManager.Play("increaseATK", AudioManager.MixerTarget.SFX, transform.position);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCSpeedUp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();

            if (unit.GetType().IsSubclassOf(typeof(Cell)) || unit.GetType().IsSubclassOf(typeof(Robot)))
            {
                unit.buffableEntity.AddBuff(scriptablSpeedUpBuff);
            }
        }
    }

    public override void Skill4()
    {
        if (timer4 <= 0)
        {
            RPCHeal();
            StartCD4();
        }
    }

    public override void Skill5()
    {
        if (timer5 <= 0)
        {
            RPCATKUp();
            StartCD5();
        }
    }

    public override void Skill6()
    {
        if (timer6 <= 0)
        {
            RPCSpeedUp();
            StartCD6();
        }
    }


    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Support Robot, which is a type of robot. I can add buffs like healing, increase attack damage and speed to friendly units within an area .";

    public override string getInfo()
    {
        return UnitInfo;
    }
}