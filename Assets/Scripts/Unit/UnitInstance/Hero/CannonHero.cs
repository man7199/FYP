using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class CannonHero : Hero, IShooter
{
    private GameObject cannonBall;
    private Transform firingPosition;

    [SerializeField] private Canvas aoecanvas;
    [SerializeField] private Image aoeRange;
    [SerializeField] private Image atkRange;
    private readonly int firestormRange = 50;
    private readonly int firestormEffectRange = 30;
    private readonly int firestormDamage = 50;
    private float cannonMovingSpeed = 17f;
    [SerializeField] private ScriptableAOEUpBuff scriptableAoeUpBuff;
    private GameObject explosion;
    private int _unitLayer;

    protected override Node SetupBehaviorTree()
    {
        return Subtree.ShooterSubtree(this);
    }

    public override Canvas AOEcircle()
    {
        return aoecanvas;
    }

    protected override void Awake()
    {
        base.Awake();
        _unitLayer = Global.UNIT_MASK;
        explodeRange = 10f;
        CD4 = 3f;
        CD5 = 3f;
        // Set UI
        sprite[3] = Resources.Load<Sprite>("Arts/UI/Icon/red_20");
        sprite[4] = Resources.Load<Sprite>("Arts/UI/Icon/red_16");
        description[3] = "AOE attack";
        description[4] = "Buff AOE";
        cannonBall = Resources.Load<GameObject>("Prefab/Projectile/Cannon Ball");
        firingPosition = transform.Find("model/firing position");
        aoecanvas = transform.Find("AoeRange").GetComponent<Canvas>();
        atkRange = transform.Find("AtkRange").Find("AttackRange").GetComponent<Image>();
        aoeRange = aoecanvas.transform.Find("AoeSkill").GetComponent<Image>();
        atkRange.enabled = false;
        aoeRange.enabled = false;
        AOEmaxRange = (float)firestormRange / 2;
        aoecanvas.transform.Find("AoeSkill").localScale = Vector3.one * firestormEffectRange;
        transform.Find("AtkRange").Find("AttackRange").localScale = Vector3.one * firestormRange;
        icon = Resources.Load<Sprite>("Icons/Robot/cannondrone");
        explosion = Resources.Load<GameObject>("Arts/Particle/BigExplosion"); 
    }


    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RpcFireStorm(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, (float)firestormEffectRange / 2, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit.Owner != Owner)
            {
                unit.TakeDamage(firestormDamage, Owner, this);
            }
        }
    }


    public override void Skill4()
    {
        if (timer4 <= 0)
        {
            UI.setClicked(false);
            aoeRange.enabled = true;
            atkRange.enabled = true;
            _isAoe = true;

            IEnumerator Effect()
            {
                while (!UI.checkClicked())
                {
                    yield return new WaitForSeconds(.1f);
                }

                aoeRange.enabled = false;
                atkRange.enabled = false;
                _isAoe = false;


                if (UI.checkCancel())
                {
                    UI.resetCancel();
                }
                else
                {
                    RpcFireStorm(aoecanvas.transform.position);
                    Runner.Spawn(explosion, aoecanvas.transform.position, Quaternion.identity);
                    AudioManager.Play("explode", AudioManager.MixerTarget.SFX, transform.position); 
                    StartCD4();
                }
            }

            StartCoroutine(Effect());
        }
    }

    public override void Skill5()
    {
        if (timer5 <= 0)
        {
            buffableEntity.AddBuff(scriptableAoeUpBuff);
            StartCD5();
        }
    }

    public void ShootProjectile(Transform target)
    {
        if (Object.HasStateAuthority)
        {
            NetworkObject cannonBall = Runner.Spawn(this.cannonBall, firingPosition.position, Quaternion.identity);
            cannonBall.GetComponent<CannonBall>().Shoot(target, cannonMovingSpeed, ATK, explodeRange, Owner, this);
            AudioManager.Play("shooting2", AudioManager.MixerTarget.SFX, transform.position);
        }
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Cannon Hero, which is a friendly robot. I can perform range area attacks and add buff to units.";

    public override string getInfo()
    {
        return UnitInfo;
    }
}