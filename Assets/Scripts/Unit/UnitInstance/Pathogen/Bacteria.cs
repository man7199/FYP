using Fusion;
using UnityEngine;

public class Bacteria : Pathogen, IMelee
{
    [Header("Bacteria Attribute")] [SerializeField]
    private int numSplit;

    [SerializeField] private NetworkPrefabRef smallerBacteria;

    [SerializeField] protected ParticleSystem particlEffect;


    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Enemy/bacteria");
    }


    public void MeleeAttack(Transform target)
    {
        target?.GetComponent<Unit>().TakeDamage(ATK, Owner, this);
        AudioManager.Play("melee_2", AudioManager.MixerTarget.SFX, transform.position);
    }

    public override bool TakeDamage(int damage, PlayerRef playerRef, Unit unit)
    {
        if (unit.GetType() == typeof(KillerCell))
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

    protected override void Death()
    {
        base.Death();
        for (int i = 0; i < numSplit; i++)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y,
                transform.position.z + Random.Range(-1, 1));
            Runner.Spawn(smallerBacteria, position, Quaternion.identity);
        }

        Debug.Log("death and spawn");
        ParticleSystem temp = Instantiate(particlEffect, transform.position, transform.rotation);
        temp.Play();
        Destroy(temp, 1);
        Destroy(gameObject);
    }

    //Setup Unit's Info
    private string UnitInfo =
        "Hello! I'm Bacteria. I'm an enemy unit and would split into several smaller bacteria after death. I have several variations with different attack power, like higher health and attack which is harder to kill.";

    public override string getInfo()
    {
        return UnitInfo;
    }
}