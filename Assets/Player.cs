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

    private XPBar xpBar;

    public Dictionary<WeaponType, WeaponProperties> weapons;

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

        if (currentLevel % 2 == 0)
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
            //    movementSpeed *= (1 + upgrade.value);
            //    break;

            case LevelUpUpgrade.UpgradeType.AttackSpeed:
                // Reduce attack cooldown (increase attack speed)
                foreach (var weapon in weapons.Values)
                {
                    weapon.cooldown *= (1 - upgrade.value);
                }
                break;

            case LevelUpUpgrade.UpgradeType.Health:
                // Increase max health
                health *= (1 + upgrade.value);
                currentHealth = health; // Heal to max
                break;

            case LevelUpUpgrade.UpgradeType.Damage:
                // Increase weapon damage
                foreach (var weapon in weapons.Values)
                {
                    weapon.damage *= (1 + upgrade.value);
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
                    weapon.range *= (1 + upgrade.value);
                }
                break;

            case LevelUpUpgrade.UpgradeType.CooldownReduction:
                // Reduce cooldowns
                foreach (var weapon in weapons.Values)
                {
                    weapon.cooldown *= (1 - upgrade.value);
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
