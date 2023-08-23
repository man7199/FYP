using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public Camera playerCamera;

    public Transform laserOrigin;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;

    LineRenderer laserLine;
    float fireTimer;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && fireTimer > fireRate)
        {
            fireTimer = 0;
            laserLine.SetPosition(0, laserOrigin.position);
            laserLine.SetPosition(1, new Vector3(169.2f, 20f, 50f));
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                //laserLine.SetPosition(1, hit.point);
            }
            else
            {
                //laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}