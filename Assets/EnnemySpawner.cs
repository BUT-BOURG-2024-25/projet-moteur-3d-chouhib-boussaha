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
            Vector2 range = new Vector2(-25, 75);

            Vector3 ennemyPosition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));

            while (Vector3.Distance(ennemyPosition, player.transform.position) < spawnDistanceToPlayer.x ||
                   Vector3.Distance(ennemyPosition, player.transform.position) > spawnDistanceToPlayer.y)
            {
                ennemyPosition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));
            }

            ennemyObject.tag = "Ennemy";
            GameObject ennemy = Instantiate(ennemyObject);
            ennemy.transform.position = ennemyPosition;
            ennemiesSpawned++;

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
