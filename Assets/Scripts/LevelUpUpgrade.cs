using UnityEngine;

[System.Serializable]
public class LevelUpUpgrade
{
    public string name; // Upgrade name
    public string description; // Upgrade description
    public Sprite icon; // Icon for the UI
    public UpgradeType type; // Type of upgrade


    public enum UpgradeType
    {
        MovementSpeed,
        AttackSpeed,
        Health,
        Damage,
        SecondLife,
        Range,
        CooldownReduction
    }
}

