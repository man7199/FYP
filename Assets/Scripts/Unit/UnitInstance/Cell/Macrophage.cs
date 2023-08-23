using Fusion;
using UnityEngine;

public class Macrophage : Cell, IMelee
{
    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Cell/macrophage");
    }

    [SerializeField]
    protected int explodeDamage = 80;

    [SerializeField] protected float explodeForce = 10;
    [SerializeField] protected float explodeRadius = 10.5f;
    [SerializeField] protected float explodeUpwardForce = 0.5f;

    [SerializeField] protected ParticleSystem explodeEffect;

    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AudioManager.Play("melee", AudioManager.MixerTarget.SFX, transform.position);
        }
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner,this);
        AudioManager.Play("melee", AudioManager.MixerTarget.SFX, transform.position);
    }



    public override bool TakeDamage
    (int damage, PlayerRef playerRef, Unit unit)
    {
            var damageTaken = damage * (100 - defense) / 100;
            HP = HP - damageTaken;
            if (HP <= 0) //unit dead
            {
                Explode();
                Invoke(nameof(Death), 0.1f);
                return true; //target dead
            }
        return false; //target not dead
    }

    public override bool TakeDamageWithoutPlayerRef
    (int damage)
    {
        HP = HP - damage;
        if (HP <= 0) //unit dead
        {
            Explode();
            Invoke(nameof(Death), 0.1f);
            return true; //target dead
        }

        return false;  //target not dead
    }

    

    private void Explode()
    {
        var explosionPosition = transform.position;
        var unitMask = LayerMask.GetMask("Unit");
        var affectedObjects = Physics.OverlapSphere(explosionPosition, explodeRadius, unitMask);
        foreach (Collider col in affectedObjects)
        {
            Rigidbody rigidBody = col.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(explodeForce, explosionPosition, explodeRadius, explodeUpwardForce,
                    ForceMode.Impulse);
            }

            // deal damage to enemy
            Unit unit = col.GetComponent<Unit>();
            if (unit != null && unit.teamType != teamType)
            {
                unit.TakeDamage(explodeDamage, Owner, this);
            }
        }
    }

    protected override void Death()
    {
        ParticleSystem temp = Instantiate(explodeEffect, transform.position, transform.rotation);
        temp.Play();
        Destroy(temp, 1);
        AudioManager.Play("explode", AudioManager.MixerTarget.SFX, transform.position);
        Destroy(gameObject);
    }

    //public void RPCParticleEffect()
    //{
    //    Runner.Spawn(minePrefab, firepoint_1.transform.position, transform.rotation);
    //}

    //[Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    //public void RPCSound()
    //{
    //    AudioManager.Play("setMine", AudioManager.MixerTarget.SFX, transform.position);
    //}

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Macrophage, which is a type of white blood cell. I'm a friendly unit who performs melee attacks to a nearby enemy, and will explode dealing area damage to all nearby enemies after death";
    public override string getInfo()
    {
        return UnitInfo;
    }
}