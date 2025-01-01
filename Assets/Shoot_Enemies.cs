using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Enemies : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    //public void Shoot(GameObject player)
    //{
    //    if (Player.Instance.weapons[WeaponType.Revolver].isReady && LookAtNearestElement.Instance.getAbsoluteNearest() != null)
    //    {

    //    Enemy enemy = LookAtNearestElement.Instance.getAbsoluteNearest().GetComponent<Enemy>();
    //    if (enemy != null)
    //    {
    //        if (bulletPrefab == null)
    //        {
    //            Debug.LogError("BulletPrefab is not assigned!");
    //            return;
    //        }

    //        if (enemy == null)
    //        {
    //            Debug.LogError("ShootPoint is not assigned!");
    //            return;
    //        }

    //        Player.Instance.DamageEnemy(enemy.gameObject, WeaponType.Revolver);

    //            // Instantiate the bullet at the shoot point
    //        GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);
    //        bullet.transform.localScale = Vector3.one * 25;

    //        bullet.transform.LookAt(enemy.gameObject.transform.position);

    //        // Add velocity to the bullet
    //        Rigidbody rb = bullet.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            rb.velocity = enemy.transform.forward * bulletSpeed;
    //        }

    //        Destroy(bullet, 3f);
    //     }
    //    }

    //}

    public void Shoot(GameObject player)
    {
        if (Player.Instance.weapons[WeaponType.Revolver].isReady && LookAtNearestElement.Instance.getAbsoluteNearest() != null)
        {
            // Get the nearest enemy
            Enemy enemy = LookAtNearestElement.Instance.getAbsoluteNearest().GetComponent<Enemy>();
            if (enemy != null)
            {
                if (bulletPrefab == null)
                {
                    Debug.LogError("BulletPrefab is not assigned!");
                    return;
                }

                // Instantiate the bullet at the player's position
                GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);


                // Scale the bullet (optional)
                bullet.transform.localScale = Vector3.one *18 ;

                // Make the bullet look at the enemy
                bullet.transform.LookAt(enemy.transform.position);

                Player.Instance.DamageEnemy(enemy.gameObject, WeaponType.Revolver);

                // Handle bullet movement and collision in this script
                StartCoroutine(HandleBullet(bullet, enemy));
            }
            else
            {
                Debug.LogError("No valid enemy found!");
            }
        }
    }

    private IEnumerator HandleBullet(GameObject bullet, Enemy targetEnemy)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            // Move the bullet toward the target
            Vector3 direction = (targetEnemy.transform.position - bullet.transform.position).normalized;
            bullet.transform.position += direction * bulletSpeed * Time.deltaTime;

            // Check for collision using Physics.OverlapSphere
            Collider[] hitColliders = Physics.OverlapSphere(bullet.transform.position, 1f);
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Ennemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        // Destroy the bullet
                        Destroy(bullet);
                        yield break;
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the bullet after its lifetime expires
        Destroy(bullet);
    }
}
