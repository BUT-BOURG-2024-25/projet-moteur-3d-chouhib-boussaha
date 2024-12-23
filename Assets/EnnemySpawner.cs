using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    [SerializeField]
    float deltaTime = -1f; //Durée de spawn
    [SerializeField]
    Vector2 spawnDistanceToPlayer = Vector2.zero; //Distance de grâce

    [SerializeField]
    public int waveEnnemyCount = -1; //Nombre d'ennemies par vague

    [SerializeField]
    float waveIncreaseFactor = -1f; //Facteur d'augmentation du nombre d'ennemis par vague

    [SerializeField]
    GameObject spawnPlane = null; //Spawner
    [SerializeField]
    GameObject ennemyObject = null; //Spawner

    private GameObject player = null;

    int ennemiesSpawned = 0;
    
    public int ennemiesLeft = 0;
    public int waveCount = 0;//Numéro de la vague en cours
    
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startWave();
    }

    public void Update()
    {
        //downEnnemyCount();
    }

    IEnumerator spawnEnnemy()
    {
        while (ennemiesSpawned < waveEnnemyCount)
        {
            Vector2 range = new Vector2(-25, 75);


            Vector3 ennemyposition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));

            while (Vector3.Distance(ennemyposition,player.transform.position) < spawnDistanceToPlayer.x || Vector3.Distance(ennemyposition, player.transform.position) > spawnDistanceToPlayer.y)
            {
                ennemyposition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));
            }

            ennemyObject.tag = "Ennemy";
            GameObject ennemy = Instantiate(ennemyObject);
            ennemy.transform.position = ennemyposition;
            ennemiesSpawned++;

            yield return new WaitForSeconds(deltaTime);
        }
    }

    //On ennemy death;
    //Used to trigger wave start
    public void downEnnemyCount()
    {
        //Kill 1 ennemy
        if (ennemiesLeft > 0)
        {
            ennemiesLeft--;
        }
        //If no ennemy remains, reset wave
        if(ennemiesLeft == 0)
        {
            startWave();
        }
    }

    //Update wave values, (increase ennemy count)
    private void startWave()
    {
            waveCount++;
            ennemiesSpawned = 0;
            waveEnnemyCount = (int) (waveEnnemyCount * waveIncreaseFactor)+1;
            ennemiesLeft = waveEnnemyCount;
            Debug.Log("Wave "+waveCount+" -- ENNEMIES: " + waveEnnemyCount);
            StartCoroutine(spawnEnnemy());
    }
}
