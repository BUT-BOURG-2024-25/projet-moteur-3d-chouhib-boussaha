using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelUpUI; // The level-up UI container

    [SerializeField]
    private Button[] upgradeButtons; // Buttons for the three upgrade choices

    [SerializeField]
    private Text[] upgradeDescriptions; // Descriptions for the upgrades

    [SerializeField]
    private Text[] upgradeNames; // Icons for the upgrades

    private LevelUpUpgrade[] allUpgrades; // List of all possible upgrades
    private LevelUpUpgrade[] currentChoices = new LevelUpUpgrade[3]; // Current level-up choices

    private bool isPaused = false;

    static public float basePercentageGain = 25f;

    private void Start()
    {
        // Define upgrades (this could be set via the Inspector or a data file)
        allUpgrades = new LevelUpUpgrade[]
        {
            //new LevelUpUpgrade { name = "Swift Feet", description = "movement speed", type = LevelUpUpgrade.UpgradeType.MovementSpeed},
            new LevelUpUpgrade { name = "Fast Hands", description = "Attack Speed", type = LevelUpUpgrade.UpgradeType.AttackSpeed},
            new LevelUpUpgrade { name = "Tanky", description = "Health", type = LevelUpUpgrade.UpgradeType.Health},
            new LevelUpUpgrade { name = "Power Strike", description = "Damage", type = LevelUpUpgrade.UpgradeType.Damage },
            //new LevelUpUpgrade { name = "Second Life", description = "Get a second life", type = LevelUpUpgrade.UpgradeType.SecondLife},
            new LevelUpUpgrade { name = "Sniper", description = "Range", type = LevelUpUpgrade.UpgradeType.Range },
            new LevelUpUpgrade { name = "Cooldown Mastery", description = "Cooldown Reduction", type = LevelUpUpgrade.UpgradeType.CooldownReduction },
        };

        levelUpUI.SetActive(false); // Hide the UI initially
    }

    public void TriggerLevelUp()
    {
        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;

        // Choose three random upgrades
        for (int i = 0; i < currentChoices.Length; i++)
        {
            currentChoices[i] = allUpgrades[Random.Range(0, allUpgrades.Length)];
        }

        // Update UI
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeDescriptions[i].text = "+" + basePercentageGain.ToString("F2") + "% " + currentChoices[i].description;
            upgradeNames[i].text = currentChoices[i].name; // Assign the icon
            int choiceIndex = i; // Prevent closure issue
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(choiceIndex));
        }

        levelUpUI.SetActive(true);
    }

    private void ApplyUpgrade(int choiceIndex)
    {
        LevelUpUpgrade selectedUpgrade = currentChoices[choiceIndex];

        // Apply the selected upgrade to the player
        Player.Instance.ApplyUpgrade(selectedUpgrade);

        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;
        basePercentageGain *= 0.95f;

        // Hide the level-up UI
        levelUpUI.SetActive(false);
    }
}
