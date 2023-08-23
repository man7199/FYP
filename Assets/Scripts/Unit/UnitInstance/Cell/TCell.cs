using UnityEngine;

public class TCell : Cell, IShooter
{
    [Header("KillerCell Attribute")] public Transform shootingPoint;
    [SerializeField] private ProjectileStatScriptableObject projectileStat;


    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Cell/Tcell");
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
            Projectile projectile = Runner
                .Spawn(projectileStat.projectilePrefab, shootingPoint.position, Quaternion.identity)
                .GetComponent<Projectile>();
            projectile.Shoot(target, projectileStat.speed, projectileStat.damage, Owner, this);
        }
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm TCell, which is a type of cell. I'm a friendly unit which can shoot tracking projectiles to attack an enemy in range.";
    public override string getInfo()
    {
        return UnitInfo;
    }
}
