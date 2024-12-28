using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // Height of the floating motion
    public float floatSpeed = 2f;       // Speed of the floating motion
    private Vector3 startPosition;

    private void Start()
    {
        // Store the initial position for floating motion
        startPosition = transform.position;
    }

    private void Update()
    {
        // Apply a smooth floating motion using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Health pack collected!");

            BuffSpawner spawner = FindObjectOfType<BuffSpawner>();
            if (spawner != null)
            {
                spawner.RemoveBuff(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
