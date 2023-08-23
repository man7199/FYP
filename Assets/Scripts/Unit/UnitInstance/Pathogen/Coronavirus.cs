using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Coronavirus : Pathogen, IShooter
{
    [Header("Corona Attribute")] [SerializeField]
    private Transform shootingPoint;

    [SerializeField] private ProjectileStatScriptableObject projectileStat;
    [SerializeField] private float infectionProb;

    [SerializeField] private NetworkPrefabRef prefab;

    protected override Node SetupBehaviorTree()
    {
        return Subtree.RecessionShooterSubtree(this);
    }

    private void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Enemy/corona");
    }
    public void MeleeAttack(Transform target)
    {
        bool isDead = target.GetComponent<Unit>().TakeDamage(ATK, Owner, this);
        AudioManager.Play("coronaAttack", AudioManager.MixerTarget.SFX, transform.position);
        if (isDead)
        {
            // killed a target, spawn unit
            if (Random.value < infectionProb)
                Runner.Spawn(prefab, target.position, Quaternion.identity);
        }
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Corona, which is a type of virus. I'm an enemy unit who shoots projectiles to attack a nearby friendly unit, and if the unit die because of me, they would turn into an enemy, so watch out! I have several variations with different attack power, like higher health and attack which is harder to kill.";
    public override string getInfo()
    {
        return UnitInfo;
    }
    
    public override bool TakeDamage(int damage, PlayerRef playerRef, Unit unit)
    {
        if (unit.GetType() == typeof(TCell))
        {
            HP = HP - (int)(damage * 1.5);
            if (target == null && unit != null)
            {
                target = unit.transform;
            }

            if (HP <= 0) //unit dead
            {
                Invoke(nameof(Death), 0.1f);
                return true; //target dead
            }

            return false; //target not dead
        }

        return base.TakeDamage(damage, playerRef, unit);
    }

    public void ShootProjectile(Transform target)
    {
        if (target == null)
        {
            Debug.Log("target null");
        }

        if (shootingPoint == null)
        {
            Debug.Log("shooting point null");
        }

        if (Object.HasStateAuthority)
        {
            Projectile projectile = Runner
                .Spawn(projectileStat.projectilePrefab, shootingPoint.position, Quaternion.identity)
                .GetComponent<Projectile>();
            Debug.Log(target);
            Debug.Log(Owner);
            projectile.Shoot(target, projectileStat.speed, projectileStat.damage, Owner, this);
        }
    }
}