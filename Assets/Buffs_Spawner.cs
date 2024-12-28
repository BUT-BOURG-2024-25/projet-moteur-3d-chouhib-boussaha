using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject healthPackPrefab; // Prefab for the health pack

    [SerializeField]
    private GameObject bombPrefab; // Prefab for the bomb

    [SerializeField]
    private Vector2 spawnRange = new Vector2(-25, 75); // X and Z range for spawning buffs

    [SerializeField]
    private float spawnInterval = 10f; // Time between spawns

    [SerializeField]
    private int maxBuffs = 5; // Maximum number of buffs on the map at any time

    [SerializeField]
    private float destroyInterval = 2f; // Time before destroying the oldest unclaimed buff

    private GameObject player;
    private List<GameObject> activeBuffs = new List<GameObject>();
    private Queue<GameObject> buffQueue = new Queue<GameObject>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerCollider");
        StartCoroutine(SpawnBuffs());
        StartCoroutine(DestroyOldestBuff());
    }

    private IEnumerator SpawnBuffs()
    {
        while (true)
        {
            if (activeBuffs.Count < maxBuffs)
            {
                SpawnRandomBuff();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomBuff()
    {

        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRange.x, spawnRange.y),
            player.transform.position.y,
            Random.Range(spawnRange.x, spawnRange.y)
        );

        GameObject buffPrefab = Random.value > 0.5f ? healthPackPrefab : bombPrefab;

        GameObject buff = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);

        if (buffPrefab == healthPackPrefab) {
            buff.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        else{
            buff.transform.localScale = new Vector3(10f, 10f, 10f);
        }
        

        activeBuffs.Add(buff);
        buffQueue.Enqueue(buff);
    }

    public void RemoveBuff(GameObject buff)
    {
        if (activeBuffs.Contains(buff))
        {
            activeBuffs.Remove(buff);
            buffQueue = new Queue<GameObject>(buffQueue);
            Destroy(buff);
        }
    }

    private IEnumerator DestroyOldestBuff()
    {
        while (true)
        {
            if (activeBuffs.Count == maxBuffs && buffQueue.Count > 0)
            {
                GameObject oldestBuff = buffQueue.Dequeue();

                if (oldestBuff != null && activeBuffs.Contains(oldestBuff))
                {
                    activeBuffs.Remove(oldestBuff);
                    Destroy(oldestBuff); 
                    SpawnRandomBuff();
                }
            }
            yield return new WaitForSeconds(destroyInterval);
        }
    }
}
