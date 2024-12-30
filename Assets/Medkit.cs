using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // Height of the floating motion
    public float floatSpeed = 2f;       // Speed of the floating motion
    private Vector3 startPosition;


    public GameObject healingPrefab;
    private GameObject healingEffect;


    private GameObject player;

    private void Start()
    {
        // Store the initial position for floating motion
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        // Apply a smooth floating motion using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.rotation *= Quaternion.Euler(0, 0.1f, 0);

        if(healingEffect != null && player !=null)
        {
            healingEffect.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z) ;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Health pack collected!");

            Player player = Player.Instance;
            if (player != null && healingEffect == null)
            {
                // Heal the player for 30% of their max health
                float healAmount = player.getHealth() * 0.3f;
                player.GainHealth(healAmount);
                Debug.Log($"Player healed for {healAmount}. Current health: {player.getCurrentHealth()}");

                healingEffect = Instantiate(healingPrefab, gameObject.transform.position, Quaternion.identity);
                healingPrefab.transform.localScale = Vector3.one * 3;
                StartCoroutine(destroyHealingEffect());
            }
        }
    }

    private IEnumerator destroyHealingEffect()
    {
        gameObject.transform.localScale = Vector3.one * 0;
        //yield return new WaitForSeconds(explosionPrefab.GetComponent<ParticleSystem>().main.duration);
        yield return new WaitForSeconds(3f);
        
            Destroy(healingEffect);

            BuffSpawner spawner = FindObjectOfType<BuffSpawner>();
            if (spawner != null)
            {
                spawner.RemoveBuff(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
    }
}
