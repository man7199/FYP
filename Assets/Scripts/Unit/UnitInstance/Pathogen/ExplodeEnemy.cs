using System;
using UnityEngine;

public class ExplodeEnemy : Pathogen, IMelee
{
    [Header("Explode Enemy")] [SerializeField]
    protected int explodeDamage = 100;

    [SerializeField] protected float explodeForce = 10;
    [SerializeField] protected float explodeRadius = 10.5f;
    [SerializeField] protected float explodeUpwardForce = 0.5f;

    [SerializeField] protected ParticleSystem explodeEffect;



    //deal explosion damage
    //potential updates: damage increase with shorted range
    private void Explode()
    {
        var explosionPosition = transform.position;
        var unitMask = LayerMask.GetMask("Unit");
        var affectedObjects = Physics.OverlapSphere(explosionPosition, explodeRadius, unitMask);
        foreach (Collider col in affectedObjects)
        {
            // exert explosive force
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
                unit.TakeDamage(explodeDamage, Owner,this);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && unit.teamType != teamType)
        {
            MeleeAttack(unit.transform);
        }
    }
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public void MeleeAttack(Transform target)
    {
        Invoke(nameof(Death), 0.1f);
    }

    protected override void Death()
    {
        Explode();
        ParticleSystem temp = Instantiate(explodeEffect, transform.position, transform.rotation);
        temp.Play();
        Destroy(temp, 1);
        AudioManager.Play("explode", AudioManager.MixerTarget.SFX, transform.position);
        Destroy(gameObject);

    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm a friendly unit. I'm a cell who will explode when close to enemy to deal area damage ";
    public override string getInfo()
    {
        return UnitInfo;
    }
}