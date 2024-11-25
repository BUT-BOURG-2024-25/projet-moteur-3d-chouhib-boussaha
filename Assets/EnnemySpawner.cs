using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    [SerializeField]
    float deltaTime = -1f; //Dur�e de spawn
    [SerializeField]
    Vector2 spawnDistanceToPlayer = Vector2.zero; //Distance de gr�ce

    [SerializeField]
    float intervalRandomRange = -1f; //Variation par rapport aux param�tres d�finis par l'al�atoire

    [SerializeField]
    GameObject spawnPlane = null; //Spawner
    void Update()
    {
        spawnEnnemy();
    }

    IEnumerator spawnEnnemy()
    {
        yield return new WaitForSeconds(deltaTime);
    }
}
