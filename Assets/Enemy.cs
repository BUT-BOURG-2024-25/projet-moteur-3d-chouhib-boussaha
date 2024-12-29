﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;
    private float currentHealth = 0f;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float damage = 0f;
    [SerializeField]
    private float attackCooldown = 0f;

    [SerializeField]
    private float range = 4f;

    [SerializeField]
    private float xpReward = 20f;

    bool canAttack = true;

    private SkinnedMeshRenderer renderer;

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
            Debug.Log("ATTACKING PLAYER");
            player.GetComponent<Player>().TakeDamage(this.damage);
            StartCoroutine(CooldownRoutine(this.attackCooldown));
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

        if (currentHealth <= 0)
        {
            Die();
        }
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

        Debug.Log("Enemy died!");
        Destroy(gameObject);
        EnnemySpawner.Instance.downEnnemyCount(); // Notify the spawner to decrease enemy count
    }

    public float getMoveSpeed()
    {
        return this.moveSpeed;
    }

    public float getRange()
    {
        return this.range;
    }

}
