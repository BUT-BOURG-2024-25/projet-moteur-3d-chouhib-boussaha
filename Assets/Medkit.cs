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

            // Heal the player for 30% of their health
            Player player = Player.Instance;
            if (player != null)
            {
                float healAmount = player.getHealth() * 0.3f; // Calculate 30% of max health
                player.GainHealth(healAmount);
                Debug.Log($"Player healed for {healAmount}. Current health: {player.getCurrentHealth()}");
            }

            BuffSpawner spawner = FindObjectOfType<BuffSpawner>();
            if (spawner != null)
            {
                spawner.RemoveBuff(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
