using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not in use script
public class MeleeAttackExplode : Cell, IMelee
    //a cell that performs melee attack and explode when dead
{
    [Header("Melee Explode Enemy")] [SerializeField]
    protected int explodeDamage = 80;

    [SerializeField] protected float explodeForce = 10;
    [SerializeField] protected float explodeRadius = 10.5f;
    [SerializeField] protected float explodeUpwardForce = 0.5f;


    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner, this);
    }


    public override bool TakeDamage
        (int damage, PlayerRef playerRef, Unit unit)
    {
        var randomNum = Random.Range(0, 99);
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

    private void Explode()
    {
        var explosionPosition = transform.position;
        var unitMask = LayerMask.GetMask("Unit");
        var affectedObjects = Physics.OverlapSphere(explosionPosition, explodeRadius, unitMask);
        foreach (Collider col in affectedObjects)
        {
            Debug.Log("Explode2");
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
                unit.TakeDamage(explodeDamage, Owner, this);
            }
        }
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm someone who performs melee attack and will explode after death";

    public override string getInfo()
    {
        return UnitInfo;
    }
}