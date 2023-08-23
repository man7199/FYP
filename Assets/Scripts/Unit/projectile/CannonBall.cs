using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using static Unit;


public class CannonBall : NetworkBehaviour

{
    private PlayerRef playerRef;
    private Unit shooter;
    private float speed;
    private int damage;
    private float explodeRange;
    private bool isLaunched = false;
    private Rigidbody rb;
    private readonly float threshold = 1f;
    public float horizontalSpeed;
    public float verticalSpeed;
    private int _unitLayer;

    public float minDistance = 2f;
    private float distance;
    private bool moveFlag = true;
    public Vector3 targetPosition;
    private Vector3 direction;
    private float g = 9.81f;
    public NetworkPrefabRef explosion;

    public void Shoot(Transform target, float speed, int damage, float explodeRange, PlayerRef playerRef, Unit shooter)
    {
        this.playerRef = playerRef;
        this.shooter = shooter;
        this.explodeRange = explodeRange;
        this.damage = damage;
        horizontalSpeed = speed;
        targetPosition = target.position;
        distance = Vector3.Distance(transform.position, this.targetPosition);
        isLaunched = true;
        direction = (target.position - transform.position).normalized;
        float needTime = distance / speed;
        verticalSpeed = g * needTime;
        _unitLayer = Global.UNIT_MASK;
    }

    private void FixedUpdate()
    {
        if (isLaunched)
        {
            verticalSpeed = verticalSpeed - g * Time.fixedDeltaTime;
            transform.Translate(direction * horizontalSpeed * Time.fixedDeltaTime, Space.World);
            transform.Translate(Vector3.up * verticalSpeed * Time.fixedDeltaTime, Space.World);
            if (Vector3.Distance(transform.position, targetPosition) < minDistance)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        Debug.Log("explode");
        Collider[] colliders = Physics.OverlapSphere(targetPosition, explodeRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit.teamType != shooter.teamType)
            {
                unit.TakeDamage(damage, playerRef, shooter);
            }
        }

        RPCShowEffect(transform.position);
        // Runner.Spawn(explosion, transform.position, Quaternion.identity);
        // AudioManager.Play("explode", AudioManager.MixerTarget.UI, transform.position);
        Destroy(gameObject);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCShowEffect(Vector3 position)
    {
        Runner.Spawn(explosion, position, Quaternion.identity);
        AudioManager.Play("explode", AudioManager.MixerTarget.UI, position);
    }
}