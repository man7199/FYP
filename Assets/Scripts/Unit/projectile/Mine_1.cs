using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_1 : NetworkBehaviour
{
    [SerializeField] float explodeRadius = 5f;
    [SerializeField] int explodeDamage= 20;

    [SerializeField] float explodeForce = 5f;
    [SerializeField] float explodeUpwardForce = 3f;
    [SerializeField] protected ParticleSystem explodeEffect;

    bool activated = false;


    public void OnTriggerEnter(Collider other)
    {
        Unit interactUnit = other.GetComponent<Unit>();
        Robot robot = other.GetComponent<Robot>();

        if (interactUnit != null)
        {
            if (robot == null)
            { 
                if(!activated) 
                {
                    Explode();
                }
            }
        }

    }


    private void Explode()
    {
        activated = true;
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

            // deal damage to all enemy and friendly targets within explode range
            Unit unit = col.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeDamage(explodeDamage, unit.Owner, unit);
            }
            ParticleSystem temp = Instantiate(explodeEffect, transform.position, transform.rotation);
            temp.Play();
            Destroy(temp, 1);
            AudioManager.Play("explode", AudioManager.MixerTarget.SFX, transform.position);
            Destroy(gameObject);

        }



    }
}
