using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    static private float health = 100f;
    private float currentHealth = 0f;

    static private float moveSpeed = 7;

    
    static private float damage = 15f;

    static private float attackCooldown = 0.5f;

    static private float range = 3.5f;

    static private float xpReward = 20f;

    bool canAttack = true;

    private SkinnedMeshRenderer renderer;

    public GameObject damageTakePrefab;
    private GameObject damageEffect;

    static public void enhanceEnemies()
    {
        health *= 1.2f;
        damage *= 1.2f;
        moveSpeed *= 1.01f;
        xpReward *= 1.10f;
    }


    public void Start()
    {
        Transform skinnedMeshChild = transform.Find("BountyHunterRIO2").transform.Find("BountyHunter_RIO_2");
        if (skinnedMeshChild != null)
        {
            renderer = skinnedMeshChild.GetComponent<SkinnedMeshRenderer>();
        }

        currentHealth = health;
    }


    public void AttackPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && canAttack)
        {
            player.GetComponent<Player>().TakeDamage(damage);
            StartCoroutine(CooldownRoutine(attackCooldown));
        }
    }

    private IEnumerator CooldownRoutine(float cooldownTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        float healthPercentage = currentHealth / health;

        Color color = new Color(1 - healthPercentage, healthPercentage, 0, 0.75f); // RGBA color based on health

        renderer.material.color = color;

        if (damageEffect != null)
        {
            Destroy(damageEffect);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            damageEffect = GameObject.Instantiate(damageTakePrefab, gameObject.transform.position, gameObject.transform.rotation);
            damageEffect.transform.localScale *= 4;

            StartCoroutine(DestroydamageEffect());

        }
    }

    private IEnumerator DestroydamageEffect()
    {
        yield return new WaitForSeconds(damageTakePrefab.GetComponent<ParticleSystem>().main.duration);
        Destroy(damageEffect);
    }

    private void Die()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.GainXP(xpReward); // Grant XP to the player
                Debug.Log($"Player gained {xpReward} XP from killing an enemy!");
            }
            else
            {
                Debug.LogError("Player script not found on the Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        Destroy(gameObject);
        EnnemySpawner.Instance.downEnnemyCount(); // Notify the spawner to decrease enemy count
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public float getRange()
    {
        return range;
    }

    

}
