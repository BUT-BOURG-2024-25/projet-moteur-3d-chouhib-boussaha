using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    private Slider xpSlider;

    public void SetMaxXP(float maxHealth)
    {
        xpSlider.maxValue = maxHealth;
        xpSlider.value = maxHealth;
    }

    public void UpdateXP(float currentHealth) { xpSlider.value = currentHealth; }
}
