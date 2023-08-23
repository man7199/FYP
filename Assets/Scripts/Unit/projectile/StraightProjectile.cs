using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class StraightProjectile : NetworkBehaviour
{
    private int damage;
    private Vector3 startPosition;
    private float range;
    public ScriptablePoisonBuff scriptablepoisonBuff;

    private PlayerRef playerRef;
    private Unit shooter;

    [SerializeField] protected NetworkPrefabRef explodeEffect;

    public void Shoot(Transform target, int damage, float speed, float range, PlayerRef playerRef, Unit shooter,ScriptablePoisonBuff buff)
    {
        scriptablepoisonBuff = buff;
        this.range = range;
        this.playerRef = playerRef;
        this.shooter = shooter;
        this.damage = damage;
        startPosition = transform.position;
        GetComponent<Rigidbody>().velocity = (target.position - transform.position).normalized * speed;
    }


    private void FixedUpdate()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= range)
        {
            Runner.Despawn(Object);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & Global.UNIT_MASK) != 0)
        {
            Unit unit = other.GetComponent<Unit>();
            if (unit.teamType != shooter.teamType)
            {
                Debug.Log("attacking");
                unit.TakeDamage(damage, playerRef, shooter);
                unit.buffableEntity.AddBuff(scriptablepoisonBuff);

                RPCShowEffect();
                Runner.Despawn(Object);
            }
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCShowEffect()
    {
        Runner.Spawn(explodeEffect, transform.position, transform.rotation);
        AudioManager.Play("explode_2", AudioManager.MixerTarget.SFX, transform.position);
        Debug.Log("attacking and play particle success");
    }
}