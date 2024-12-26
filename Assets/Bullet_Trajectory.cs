using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Trajectory : MonoBehaviour{

    public float bulletSpeed = 20f; // Speed of the bullet

    void Update()
    {
        // Move the bullet forward every frame
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}
