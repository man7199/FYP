using System;
using UnityEngine;

public class Virus : Pathogen, IMelee
{
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>().TakeDamage(ATK, Owner, this);
        AudioManager.Play("melee", AudioManager.MixerTarget.SFX, transform.position);
    }
    private void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Enemy/virus");
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Virus. I'm a enemy unit who melee attacks a nearby unit. I have several variations with different attack power, like higher health and attack which is harder to kill.";
    public override string getInfo()
    {
        return UnitInfo;
    }
}