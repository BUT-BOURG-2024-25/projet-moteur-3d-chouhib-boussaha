using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{
    Auto,
    Revolver,
    // Add more weapons here
}

[Serializable]
public class WeaponProperties
{
    public float damage;
    public float cooldown;
    public bool isReady = true;
    public float range;
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

    //Healthbar
    private HealthBar healthBar;

    public Dictionary<WeaponType, WeaponProperties> weapons;

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health); // Initialize the health bar
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
            { WeaponType.Revolver, revolverWeapon }
        };
        this.gameObject.tag = "Player";
    }

    public void TakeDamage(float damage)
    {
        this.currentHealth -= damage;
        Debug.Log("Player HP : "+currentHealth);

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        if (this.currentHealth <= 0)
        {

            SurvivalTimer survivalTimer = FindObjectOfType<SurvivalTimer>();
            if (survivalTimer != null)
            {
                survivalTimer.StopTimer();
                Debug.Log($"Player survived for: {survivalTimer.GetElapsedTime()} seconds.");
            }

            Debug.Log("RIP :(");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //TODO UIMANAGER 
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

    public void DamageEnemy(GameObject enemy, WeaponType weaponType)
    {
        if (!weapons[weaponType].isReady)
        {
            Debug.Log($"Weapon {weaponType} cooldown : cant attack");
            return;
        }

        Enemy enemyObject = enemy.GetComponent<Enemy>();
        if (enemyObject != null)
        {
            Debug.Log($"Damaged ennemy using {weaponType} - {weapons[weaponType].damage} dmg");
            float weaponDamage = weapons[weaponType].damage;
            if(weaponType== WeaponType.Auto)
            enemyObject.TakeDamage(weaponDamage);
            StartCooldown(weaponType);
        }
    }

    private void StartCooldown(WeaponType weaponType)
    {
        WeaponProperties weapon = weapons[weaponType];
        weapon.isReady = false;
        StartCoroutine(CooldownRoutine(weaponType, weapon.cooldown));
    }

    private IEnumerator CooldownRoutine(WeaponType weaponType, float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        weapons[weaponType].isReady = true;
    }

    public float getHealth(){  return health; }

    public float getCurrentHealth() {  return currentHealth; }
}
