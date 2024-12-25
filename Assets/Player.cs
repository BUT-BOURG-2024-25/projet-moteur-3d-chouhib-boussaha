using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    //Health
    [SerializeField]
    private float health = -1f;
    private float currentHealth = -1f;

    //[SerializeField]
    //private float healthCoeff = -1f;

    //XP
    private int currentLevel = 1;
    private float currentXP = 0;

    [SerializeField]
    private float nextLevelXP = -1f;

    //[SerializeField]
    //private float xpCoeff = -1f;

    //Weapons
    [SerializeField]
    private float mainWeaponDamage = -1f;

    [SerializeField]
    private float mainWeaponCooldown = -1f;

    public bool canAttack = true; //Attack an ennemy when the cd is down and nobody is in range


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

    public void TakeDamage(float damage)
    {
        this.currentHealth -= damage;
    }

    public void GainHealth(float health)
    {
        this.currentHealth += health;
    }

    public void XpUP(float xp)
    {
        this.currentXP += xp;
        if (this.currentXP >= nextLevelXP)
        {
            this.currentLevel += 1;
            this.currentXP = nextLevelXP % this.currentXP;
        }
        //TODO Reset nextLevelXP
        //TODO Power up
        //TODO Health increase
    }

    public void attackEnnemy(GameObject ennemy)
    {
        if (this.canAttack)
        {
            SetCooldown(this.mainWeaponCooldown);
            Destroy(ennemy);
            EnnemySpawner.Instance.downEnnemyCount();
        }

    }

    public void SetCooldown(float cooldownTime)
    {
        StartCoroutine(CooldownRoutine(cooldownTime));
    }

    private IEnumerator CooldownRoutine(float cooldownTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true; 
    }

}
