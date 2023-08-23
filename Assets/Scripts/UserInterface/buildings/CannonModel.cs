using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class CannonModel : NetworkBehaviour
{
    // target the gun will aim at
    public Transform go_target;

    // Gameobjects need to control rotation and aiming
    public Transform go_baseRotation;

    
    // Distance the turret can aim and fire from
    public float firingRange;

    // Particle system for the muzzel flash
    public ParticleSystem muzzelFlash;
    public ParticleSystem muzzelFlash2;
    public ParticleSystem explosion;
    public Animator animator;
    // Used to start and stop the turret firing
    public bool canFire = false;
    public bool CanAttack = false;
    
    void Start()
    {
        // Set the firing range distance
        this.GetComponent<SphereCollider>().radius = firingRange;
        if (muzzelFlash.isPlaying)
        Particle_Stop();
    }

    void Update()
    {
        AimAndFire();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position to show the firing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }

    // Detect an Enemy, aim and fire
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy"&& !canFire && CanAttack)
        {
            go_target = other.transform;
            canFire = true;
            GetComponent<Cannon>().EnemyFound();
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && !canFire && CanAttack)
        {
            go_target = other.transform;
            canFire = true;
            GetComponent<Cannon>().EnemyFound();
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Transform>() == go_target)
        {
            canFire = false;
        }
    }

    void AimAndFire()
    {
        
        // if can fire turret activates
        if (canFire)
        {
            if (go_target == null)
            {
                canFire = false;
            }
            else
            {
                // aim at enemy
                Vector3 baseTargetPostition = new Vector3(go_target.position.x, this.transform.position.y, go_target.position.z);

                go_baseRotation.transform.LookAt(baseTargetPostition);
            }
            // start particle system 
            if (!muzzelFlash.isPlaying)
            {
                Particle_Start();
            }
        }
        else
        {
            if (muzzelFlash.isPlaying)
            {
                Particle_Stop();
            }
        }
    }
    void Particle_Start() {
        muzzelFlash.Play();
        muzzelFlash2.Play();
        animator.enabled=true;
    }
    void Particle_Stop() {
        muzzelFlash.Stop();
        muzzelFlash2.Stop();
        animator.enabled=false;
        
    }
}