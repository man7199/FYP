using Fusion;
using UnityEngine;

public class ArtilleryRobot : Robot, IShooter
{
    [Header("ArtilleryRobot Attribute")][SerializeField] private GameObject cannonBall;
    private float cannonMovingSpeed = 17f;
    [SerializeField] private Transform shootingPoint;

    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Robot/artilleryrobot");

    }
    protected override Node SetupBehaviorTree()
    {
        return Subtree.ShooterSubtree(this);
    }

    public override string getInfo()
    {
        return "Hello! I'm Artillery Robot, which is a type of robot. I'm a friendly unit which can shoot projectiles within a huge range, and the projectile deals area damage to enemies near the projectile drop point.";
    }

    public void ShootProjectile(Transform target)
    {
        if (Object.HasStateAuthority)
        {
            NetworkObject cannonBall = Runner.Spawn(this.cannonBall, shootingPoint.position, Quaternion.identity);
            cannonBall.GetComponent<CannonBall>().Shoot(target, cannonMovingSpeed, ATK, explodeRange, Owner, this);
        }
    }
}