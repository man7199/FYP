using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Tank : Hero, IMelee
{
    [Header("Smash")] [SerializeField] private float smashRange = 40f;
    [SerializeField] private int smashDamage;
    [SerializeField] private ScriptableStunBuff _scriptableStunBuff;
    [Header("retaliate")] [SerializeField] private ScriptableRetaliateBuff _scriptableRetaliateBuff;

    [SerializeField] private Canvas aoecanvas;
    [SerializeField] private Image aoeRange;
    private int _unitLayer;

    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public override Canvas AOEfixed()
    {
        return aoecanvas;
    }

    protected override void Awake()
    {
        base.Awake();
        _unitLayer = Global.UNIT_MASK;
        CD4 = 3f;
        CD5 = 3f;
        // Set UI
        sprite[3] = Resources.Load<Sprite>("Arts/UI/Icon/blue_23");
        sprite[4] = Resources.Load<Sprite>("Arts/UI/Icon/blue_20");
        description[3] = "Stun";
        description[4] = "Retaliate";
        icon = Resources.Load<Sprite>("Icons/Robot/tankdrone");
        aoecanvas = transform.Find("AoeRange").GetComponent<Canvas>();
        aoeRange = aoecanvas.transform.Find("AoeSkill").GetComponent<Image>();
        aoeRange.enabled = false;
        aoecanvas.transform.Find("AoeSkill").localScale = Vector3.one * smashRange;
    }

    public override bool TakeDamage(int damage, PlayerRef playerRef, Unit unit)
    {
        if (buffableEntity.IsRetaliateActive)
        {
            Buff retaliateBuff = buffableEntity.GetBuff(typeof(RetaliateBuff));
            ScriptableRetaliateBuff retaliateBuffBuffData = (ScriptableRetaliateBuff)retaliateBuff.BuffData;
            int retaliateDamage = (int)(damage * retaliateBuffBuffData.retaliatePercentage);
            if (unit != null)
            {
                unit.TakeDamage(retaliateDamage, Owner, this);
            }

            return base.TakeDamage(damage - retaliateDamage, playerRef, unit);
        }
        else
        {
            return base.TakeDamage(damage, playerRef, unit);
        }
    }


    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner, this);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCSmash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, smashRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit.teamType != teamType)
            {
                unit.buffableEntity.AddBuff(_scriptableStunBuff);
                unit.TakeDamage(smashDamage, Owner, this);
            }
        }

        AudioManager.Play("smash", AudioManager.MixerTarget.SFX, transform.position);
    }

    public void Retaliate()
    {
        buffableEntity.AddBuff(_scriptableRetaliateBuff);
    }


    public override void Skill4()
    {
        if (timer4 <= 0)
        {
            RPCSmash();
            StartCD4();
        }
    }

    public override void Skill5()
    {
        if (timer5 <= 0)
        {
            Retaliate();
            StartCD5();
        }
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Tank Hero, which is a friendly robot. I can add buff to friendly, deal area damage to enemies near me, and I have high health, which is able to bear a lot of damage.";

    public override string getInfo()
    {
        return UnitInfo;
    }
}