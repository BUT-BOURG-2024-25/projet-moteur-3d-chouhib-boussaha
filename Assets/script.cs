using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float floatAmplitude = 0.5f; 
    public float floatSpeed = 2f;       
    private Vector3 startPosition;

    private void Start(){
        startPosition = transform.position;
    }

    private void Update(){
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.rotation *= Quaternion.Euler(0, 0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Bomb collected!");

            // Notify the BuffSpawner or remove the bomb
            BuffSpawner spawner = FindObjectOfType<BuffSpawner>();
            if (spawner != null)
            {
                spawner.RemoveBuff(gameObject);
            }

            Destroy(gameObject); // Destroy the bomb for now
        }
    }
}

