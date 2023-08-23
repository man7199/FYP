    using System;
using UnityEngine;

public class ExplodeVirus : Pathogen, IMelee
{
    [Header("Explode Enemy")] [SerializeField]
    protected int explodeDamage = 100;

    [SerializeField] protected float explodeRadius = 10.5f;
    [SerializeField] protected ParticleSystem explodeEffect;

    private void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Enemy/virus");
    }
    //deal explosion damage
    //potential updates: damage increase with shorted range
    private void Explode()
    {
        var explosionPosition = transform.position;
        var unitMask = LayerMask.GetMask("Unit");
        var affectedObjects = Physics.OverlapSphere(explosionPosition, explodeRadius, unitMask);
        foreach (Collider col in affectedObjects)
        {
            // deal damage to enemy
            Unit unit = col.GetComponent<Unit>();
            if (unit != null && unit.teamType != teamType)
            {
                unit.TakeDamage(explodeDamage, Owner,this);
            }
        }

        ParticleSystem temp = Instantiate(explodeEffect, transform.position, transform.rotation);
        temp.Play();
        Destroy(temp, 1);
        AudioManager.Play("explode", AudioManager.MixerTarget.SFX, transform.position);
        Destroy(gameObject);
        Invoke(nameof(Death), 0.1f);
    }

    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>().TakeDamage(ATK, Owner, this);
    }
    protected override void Death()
    {
        base.Death();
        Explode();
        Destroy(gameObject);
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm explode virus, which is a type of virus. I'm your enemy who will explode when close to your friendly units to deal area damage.";
    public override string getInfo()
    {
        return UnitInfo;
    }
}