using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    private GameObject explosionEffect;
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 2f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        if (explosionPrefab == null)
        {
            Debug.LogError("Explosion prefab null !");
        }
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.rotation *= Quaternion.Euler(0, 0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider") && explosionEffect == null)
        {
            Debug.Log("Bomb collected!");
            explosionEffect = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            explosionPrefab.transform.localScale = Vector3.one * 7;
            StartCoroutine(destroyExplosionEffect());
        }
    }

    private IEnumerator destroyExplosionEffect()
    {
        gameObject.transform.localScale = Vector3.one * 0;
        //yield return new WaitForSeconds(explosionPrefab.GetComponent<ParticleSystem>().main.duration);
        yield return new WaitForSeconds(explosionPrefab.GetComponent<ParticleSystem>().main.duration);
        Collider[] hitColliders = Physics.OverlapSphere(explosionEffect.transform.position, explosionPrefab.transform.localScale.x * 3);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Ennemy"))
            {
                Debug.Log("DAMAGED ENNEMY BOMB");
                hitColliders[i].GetComponent<Enemy>().TakeDamage(Player.Instance.weapons[WeaponType.Auto].damage * 5);
            }
        }
            Destroy(explosionEffect);

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

