using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    private Slider xpSlider;

    [SerializeField]
    private Text xpText; // Optional: Text to display current XP

    public void SetMaxXP(float maxXP)
    {
        xpSlider.minValue = 0; // Ensure min value is 0
        xpSlider.maxValue = maxXP; // Set the max value
        xpSlider.value = 0; // Initialize the slider to empty

        if (xpText != null)
        {
            xpText.text = $"0 / {Mathf.FloorToInt(maxXP)}";
        }
    }

    public void UpdateXP(float currentXP, float maxXP)
    {
        xpSlider.value = Mathf.Clamp(currentXP, 0, maxXP); // Ensure value is clamped within range

        if (xpText != null)
        {
            xpText.text = $"{Mathf.FloorToInt(currentXP)} / {Mathf.FloorToInt(maxXP)}";
        }
    }
}
