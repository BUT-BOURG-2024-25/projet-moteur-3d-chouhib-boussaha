using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Enemies : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    public void Shoot(GameObject player)
    {
        if (Player.Instance.weapons[WeaponType.Revolver].isReady && LookAtNearestElement.Instance.getAbsoluteNearest() != null)
        {
        
        Enemy enemy = LookAtNearestElement.Instance.getAbsoluteNearest().GetComponent<Enemy>();
        if (enemy != null)
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("BulletPrefab is not assigned!");
                return;
            }

            if (enemy == null)
            {
                Debug.LogError("ShootPoint is not assigned!");
                return;
            }
            
            Player.Instance.DamageEnemy(enemy.gameObject, WeaponType.Revolver);

                // Instantiate the bullet at the shoot point
            GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);
            bullet.transform.localScale = Vector3.one * 25;

            bullet.transform.LookAt(enemy.gameObject.transform.position);

            // Add velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = enemy.transform.forward * bulletSpeed;
            }
            //Destroy(bullet, 3f);
         }
        }

    }
}
