using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public Image experienceBar;
    public TextMeshProUGUI health;
    public TextMeshProUGUI level;
    public TextMeshProUGUI rateOfFire;
    public TextMeshProUGUI fireDistance;
    public TextMeshProUGUI firePower;

    private float currentHealth;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        if(currentHealth < 0 ) currentHealth = 0;
        health.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdateExperienceBar(float currentExperience, float experienceToNextLevel)
    {
        experienceBar.fillAmount = currentExperience / experienceToNextLevel;
    }

    public void UpdateLevel(int newLevel, float newRateOfFire, float newFireDistance, float newFirePower, float newMaxHealth)
    {
        level.text = $"Lv.{newLevel.ToString()}";
        rateOfFire.text = $"{newRateOfFire.ToString("0.0")} / sec";
        fireDistance.text = $"{newFireDistance} μm";
        firePower.text = $"{newFirePower.ToString("0.0")}";
        health.text = $"{currentHealth}/{newMaxHealth}";
    }
}
