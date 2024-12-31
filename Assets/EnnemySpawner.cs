using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    public static EnnemySpawner Instance { get; private set; }

    [SerializeField]
    float deltaTime = -1f; // Durée de spawn

    [SerializeField]
    Vector2 spawnDistanceToPlayer = Vector2.zero; // Distance de grâce

    [SerializeField]
    public int waveEnnemyCount = -1; // Nombre d'ennemies par vague

    [SerializeField]
    float waveIncreaseFactor = -1f; // Facteur d'augmentation du nombre d'ennemis par vague

    [SerializeField]
    GameObject spawnPlane = null; // Spawner
    [SerializeField]
    GameObject ennemyObject = null; // Spawner

    [SerializeField]
    private GameObject damageTakePrefab; //:(

    private GameObject player = null;

    int ennemiesSpawned = 0;

    public int ennemiesLeft = 0;
    public int waveCount = 0; // Numéro de la vague en cours

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startWave();
    }

    IEnumerator spawnEnnemy()
    {
        while (ennemiesSpawned < waveEnnemyCount)
        {
            Vector3 ennemyPosition = MapGenerator.Instance.GetRandomPositionWithinBounds(); 

            
            int maxAttempts = 10; 
            int attempts = 0;

            while ((Vector3.Distance(ennemyPosition, player.transform.position) < spawnDistanceToPlayer.x ||
                    Vector3.Distance(ennemyPosition, player.transform.position) > spawnDistanceToPlayer.y) &&
                   attempts < maxAttempts)
            {
                ennemyPosition = MapGenerator.Instance.GetRandomPositionWithinBounds();
                attempts++;
            }

            
            if (attempts == maxAttempts)
            {
                Debug.LogWarning("Could not find a valid spawn position for an enemy after multiple attempts.");
            }

            
            GameObject ennemy = Instantiate(ennemyObject, ennemyPosition, Quaternion.identity);
            ennemy.tag = "Ennemy";

            
            ennemiesSpawned++;
            //ennemiesLeft++;

            
            yield return new WaitForSeconds(deltaTime);
        }
    }

    // On ennemy death;
    // Used to trigger wave start
    public void downEnnemyCount()
    {
        if (ennemiesLeft > 0)
        {
            ennemiesLeft--;
            UIManager.Instance.setEnemiesLeft(ennemiesLeft);
            Debug.Log("Wave " + waveCount + " -- ENNEMIES: " + (ennemiesLeft).ToString());
        }

        // If no ennemy remains, reset wave
        if (ennemiesLeft == 0)
        {
            Enemy.enhanceEnemies();
            startWave();
        }
    }

    // Update wave values, (increase ennemy count)
    private void startWave()
    {
        waveCount++;

        ennemiesSpawned = 0;
        waveEnnemyCount = (int)(waveEnnemyCount * waveIncreaseFactor) + 1;

        Debug.Log("Wave " + waveCount + " -- ENNEMIES: " + waveEnnemyCount);
        ennemiesLeft = waveEnnemyCount;

        
        UIManager.Instance.setEnemiesLeft(ennemiesLeft);
        UIManager.Instance.setWaveNumber(waveCount);

        StartCoroutine(spawnEnnemy());
    }
}
