using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Enemies : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;

    public void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPrefab is not assigned!");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogError("ShootPoint is not assigned!");
            return;
        }

        // Instantiate the bullet at the shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Add velocity to the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootPoint.forward * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }
}
