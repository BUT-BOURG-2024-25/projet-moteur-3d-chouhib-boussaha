﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public enum WeaponType
{
    Auto,
    Revolver,
    Stress,
    Dodge,
    AOESlash
}

[Serializable]
public class WeaponProperties
{
    public float damage;
    public float cooldown;
    public bool isReady = true;
    public float range;
    public float duration = 0;
}

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }


    // Health
    [SerializeField] 
    private float health = 100f;
    private float currentHealth = 100f;

    // XP
    private int currentLevel = 1;
    private float currentXP = 0;

    [SerializeField] private float nextLevelXP = 100f;

    // Weapons
    [SerializeField] private WeaponProperties autoWeapon = new WeaponProperties { };
    [SerializeField] private WeaponProperties revolverWeapon = new WeaponProperties { };
    [SerializeField] private WeaponProperties stressWeapon = new WeaponProperties { };
    [SerializeField] private WeaponProperties dodgeWeapon = new WeaponProperties { };
    [SerializeField] private WeaponProperties aoeslashWeapon = new WeaponProperties { };


    //Healthbar
    private HealthBar healthBar;

    private XPBar xpBar;

    public Dictionary<WeaponType, WeaponProperties> weapons;

    [SerializeField]
    private GameObject autoAnimationPrefab;
    private GameObject autoEffect;

    [SerializeField]
    private GameObject dodgeAnimationPrefab;
    private GameObject dodgeEffect;

    [SerializeField]
    private GameObject stressAnimationPrefab;
    private GameObject stressEffect;

    [SerializeField]
    private GameObject aoeslashAnimationPrefab;
    private GameObject aoeslashEffect;
    private bool aoeattack = false;

    private bool targetable = true; //Dodging mechanic
 

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health); // Initialize the health bar
        }
        xpBar = FindObjectOfType<XPBar>();
        if (xpBar != null)
        {
            xpBar.SetMaxXP(currentXP); // Initialize the health bar
        }

        UIManager.Instance?.SetPlayerLevel(currentLevel);
    }

    private void Update()
    {
     if(dodgeEffect != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            dodgeEffect.transform.position = player.transform.position;
        }
        if (stressEffect != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            stressEffect.transform.position = player.transform.position;
        }

        if (aoeslashEffect != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            aoeslashEffect.transform.position = player.transform.position;
            if (aoeattack)
            {
                aoeattack = false;

                Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, weapons[WeaponType.AOESlash].range);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Ennemy"))
                    {

                        this.DamageEnemy(hitColliders[i].gameObject, WeaponType.AOESlash);
                    }
                }
            }
        }
    }


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
        weapons = new Dictionary<WeaponType, WeaponProperties>
        {
            { WeaponType.Auto, autoWeapon },
            { WeaponType.Revolver, revolverWeapon },
            { WeaponType.Stress, stressWeapon },
            { WeaponType.Dodge, dodgeWeapon },
            { WeaponType.AOESlash, aoeslashWeapon },
        };
        this.gameObject.tag = "Player";
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player HP: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0 && !GameOverManager.Instance.IsGameOver())
        {
            Debug.Log("Player died!");

            // Stop the survival timer and get the elapsed time
            SurvivalTimer survivalTimer = FindObjectOfType<SurvivalTimer>();
            float survivalTime = 0f;
            if (survivalTimer != null)
            {
                survivalTimer.StopTimer();
                survivalTime = survivalTimer.GetElapsedTime();
            }

            // Show the game-over popup
            GameOverManager.Instance.ShowDeathPopup(survivalTime);
        }
    }


    public void GainHealth(float healthAmount){
        currentHealth += healthAmount;
        currentHealth = Mathf.Min(currentHealth, health); // Ensure health doesn't exceed max

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        Debug.Log($"Player healed! Current Health: {currentHealth}/{health}");

    }

    public void GainXP(float xp)
    {
        currentXP += xp;
        Debug.Log($"Player gained {xp} XP! Current XP: {currentXP}/{nextLevelXP}");

        if (xpBar != null)
        {
            xpBar.UpdateXP(currentXP,nextLevelXP); // Update the XP bar
        }

        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP = currentXP - nextLevelXP; // Carry over extra XP
        nextLevelXP = nextLevelXP * 1.3f;

        //Enhancements
        this.health *= 1.05f;
        this.currentHealth *= 1.05f;


        foreach (WeaponType weapon in (WeaponType[])Enum.GetValues(typeof(WeaponType)))
        {
            weapons[weapon].cooldown *= 0.99f;
            weapons[weapon].damage *= 1.1f;
        }

        Debug.Log($"Level Up! Player is now level {currentLevel}. Next level XP: {nextLevelXP}");

        if (xpBar != null)
        {
            xpBar.SetMaxXP(nextLevelXP); // Update the XP bar for the new level
            xpBar.UpdateXP(currentXP, nextLevelXP); // Refresh the XP bar
        }

        UIManager.Instance?.SetPlayerLevel(currentLevel);

        if (currentLevel % 3 == 0)
        {
            FindObjectOfType<LevelUpManager>().TriggerLevelUp();
        }
    }

    public void ApplyUpgrade(LevelUpUpgrade upgrade)
    {
        switch (upgrade.type)
        {
            //case LevelUpUpgrade.UpgradeType.MovementSpeed:
            //    // Increase movement speed by the given percentage
            //    movementSpeed *= (1 + LevelUpManager.basePercentageGain);
            //    break;

            case LevelUpUpgrade.UpgradeType.AttackSpeed:
                // Reduce attack cooldown (increase attack speed)
                foreach (var weapon in weapons.Values)
                {
                    weapon.cooldown *= (1 - LevelUpManager.basePercentageGain / 100);
                }
                break;

            case LevelUpUpgrade.UpgradeType.Health:
                // Increase max health
                health *= (1 + LevelUpManager.basePercentageGain / 100);
                currentHealth = health; // Heal to max
                break;

            case LevelUpUpgrade.UpgradeType.Damage:
                // Increase weapon damage
                foreach (var weapon in weapons.Values)
                {
                    weapon.damage *= (1 + LevelUpManager.basePercentageGain/100);
                }
                break;

            //case LevelUpUpgrade.UpgradeType.SecondLife:
            //    // Implement second-life logic (e.g., resurrect the player on death)
            //    hasSecondLife = true;
            //    break;

            case LevelUpUpgrade.UpgradeType.Range:
                // Increase weapon range
                foreach (var weapon in weapons.Values)
                {
                    weapon.range *= (1 + LevelUpManager.basePercentageGain / 100);
                }
                break;

            case LevelUpUpgrade.UpgradeType.CooldownReduction:
                // Reduce cooldowns
                foreach (var weapon in weapons.Values)
                {
                    weapon.cooldown *= (1 - LevelUpManager.basePercentageGain / 100);
                }
                break;
        }
    }


    public void DamageEnemy(GameObject enemy, WeaponType weaponType)
    {
        if (!weapons[weaponType].isReady)
        {
            if(weaponType != WeaponType.Auto)
            Debug.Log($"Weapon {weaponType} cooldown : cant attack");
            return;
        }

        Enemy enemyObject = enemy.GetComponent<Enemy>();
        if (enemyObject != null)
        {
            if (weaponType == WeaponType.AOESlash)
            {
                float weaponDamage = weapons[WeaponType.Auto].damage;
                enemyObject.TakeDamage(weaponDamage);
            }
            else
            {
                float weaponDamage = weapons[weaponType].damage;
                if (weaponType == WeaponType.Auto)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    enemyObject.TakeDamage(weaponDamage);
                    if (autoEffect != null)
                    {
                        Destroy(autoEffect);
                    }
                    autoEffect = GameObject.Instantiate(autoAnimationPrefab, player.transform.position, player.transform.rotation);
                    autoEffect.transform.localScale *= weapons[WeaponType.Auto].range / 5;
                }
                StartCooldown(weaponType);
            }
        }
    }

    private void StartCooldown(WeaponType weaponType)
    {
        WeaponProperties weapon = weapons[weaponType];
        if(weaponType!=WeaponType.AOESlash)
        weapon.isReady = false;
        if(weaponType == WeaponType.Revolver)
        {
            UIManager.Instance.setShootingButtonState(false);
        }

        StartCoroutine(CooldownRoutine(weaponType, weapon.cooldown));
    }

    private IEnumerator CooldownRoutine(WeaponType weaponType, float cooldownTime)
    {

        if (weaponType == WeaponType.Stress)
        {
            UIManager.Instance.setStressButtonState(true, false);
            if (stressEffect != null)
            {
                Destroy(stressEffect);
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            stressEffect = GameObject.Instantiate(stressAnimationPrefab, player.transform.position, player.transform.rotation);
            stressEffect.transform.localScale *= 2;
            yield return new WaitForSeconds(weapons[WeaponType.Stress].duration);

            Destroy(stressEffect);

            UIManager.Instance.setStressButtonState(false, true);
            putStress(false);
        }
        else if (weaponType == WeaponType.Dodge)
        {
            if (dodgeEffect != null)
            {
                Destroy(dodgeEffect);
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            dodgeEffect = GameObject.Instantiate(dodgeAnimationPrefab, player.transform.position, player.transform.rotation);
            dodgeEffect.transform.localScale *= 2;

            this.targetable = false;
            UIManager.Instance.setDodgeButtonState(true, false);
            yield return new WaitForSeconds(weapons[WeaponType.Dodge].duration);
            Destroy(dodgeEffect);

            this.targetable = true;
            UIManager.Instance.setDodgeButtonState(false, true);
        }

        else if (weaponType == WeaponType.AOESlash)
        {
            if (aoeslashEffect != null)
            {
                Destroy(aoeslashEffect);
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");


            aoeslashEffect = GameObject.Instantiate(aoeslashAnimationPrefab, player.transform.position, player.transform.rotation);
            aoeslashEffect.transform.localScale *= 2;

            UIManager.Instance.setAOEButtonState(true, false);

            //Total duration : 4s
            float ticks =4/weapons[WeaponType.AOESlash].duration;

            for(int i =0; i< ticks; i++)
            {
                aoeattack = true;
                yield return new WaitForSeconds(weapons[WeaponType.AOESlash].duration);
            }
            Destroy(aoeslashEffect);
            weapons[WeaponType.AOESlash].isReady = false;
            UIManager.Instance.setAOEButtonState(false, true);
        }

        yield return new WaitForSeconds(cooldownTime);

        switch (weaponType)
        {
            case WeaponType.Auto:
                Destroy(autoEffect);
                break;
            case WeaponType.Revolver:
                UIManager.Instance.setShootingButtonState(true); 
                break;
            case WeaponType.Stress:
                UIManager.Instance.setStressButtonState(false,false);
                break;
            case WeaponType.Dodge:
                UIManager.Instance.setDodgeButtonState(false, false);
                break;
            case WeaponType.AOESlash:
                UIManager.Instance.setAOEButtonState(false, false);
                break;
        }

        weapons[weaponType].isReady = true;
    }

   
    public float getHealth(){  return health; }

    public float getCurrentHealth() {  return currentHealth; }

    public void useStressSpell()
    {
        if (weapons[WeaponType.Stress].isReady)
        {
            putStress(true);
            StartCooldown(WeaponType.Stress);
        }
    }

    public void putStress(bool state)
    {
        foreach (WeaponType weapon in (WeaponType[])Enum.GetValues(typeof(WeaponType)))
        {
            if (weapon != WeaponType.Stress)
            {
                if (state)
                {
                    weapons[weapon].cooldown *= 0.6f;
                }
                else
                {
                    weapons[weapon].cooldown /= 0.6f;
                }
            }
        }
    }

    public void useDodgeSpell()
    {
        if (weapons[WeaponType.Dodge].isReady)
        {
            StartCooldown(WeaponType.Dodge);
        }
    }

    public void useAOESpell()
    {
        if (weapons[WeaponType.AOESlash].isReady)
        {
            StartCooldown(WeaponType.AOESlash);
        }
    }
}
