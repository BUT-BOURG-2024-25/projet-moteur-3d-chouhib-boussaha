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
    float intervalRandomRange = -1f; //Variation par rapport aux paramètres définis par l'aléatoire

    [SerializeField]
    GameObject spawnPlane = null; //Spawner
    [SerializeField]
    GameObject ennemyObject = null; //Spawner

    private GameObject player = null;

    public void Start()
    {
        Debug.Log("SPAWNING ENNEMY START");
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(spawnEnnemy());

    }
    IEnumerator spawnEnnemy()
    {
        while (true)
        {
            Debug.Log("SPAWNING ENNEMY");
            Vector2 range = new Vector2(-25, 75);


            Vector3 ennemyposition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));

            while (Vector3.Distance(ennemyposition,player.transform.position) < spawnDistanceToPlayer.x || Vector3.Distance(ennemyposition, player.transform.position) > spawnDistanceToPlayer.y)
            {
                ennemyposition = new Vector3(Random.Range(range.x, range.y), player.transform.position.y, Random.Range(range.y, range.x));
            }

            GameObject ennemy = Instantiate(ennemyObject);
            ennemy.transform.position = ennemyposition;
            yield return new WaitForSeconds(deltaTime);
        }
    }
}
