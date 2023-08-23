using Fusion;
using UnityEngine;
using Utils;

[RequireComponent(typeof(LineRenderer))]

public class ShooterRobot : Robot, IShooter
{
    [Header("Shooter robot Attribute")] [SerializeField]private Transform shootingPoint;
    [SerializeField] private NetworkPrefabRef projectile;
    [SerializeField] private float projectileSpeed;
    public ScriptablePoisonBuff scriptablepoisonBuff;

    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Robot/range_robot");

    }
    protected override Node SetupBehaviorTree()
    {
        return Subtree.ShooterSubtree(this);

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
            StraightProjectile projectile = Runner
                .Spawn(this.projectile, shootingPoint.position, Quaternion.identity)
                .GetComponent<StraightProjectile>();
            projectile.Shoot(target,ATK, projectileSpeed,attackRange,  Owner, this,scriptablepoisonBuff);
        }
    }

    public override string getInfo()
    {
        return "Hello! I'm Shooter Robot, which is a type of robot. I'm a friendly unit which can shoot projectiles within a shorter range than the Artillery Robot. The projectile deals area damage to enemies near the projectile drop point.";
    }
}
