using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Trajectory : MonoBehaviour{

    public float bulletSpeed = -1f; // Speed of the bullet

    public void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position,1);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Ennemy"))
            {
                hitColliders[i].GetComponent<Enemy>().TakeDamage(Player.Instance.weapons[WeaponType.Revolver].damage);
                Destroy(gameObject);
            }
        }
        // Move the bullet forward every frame
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}
