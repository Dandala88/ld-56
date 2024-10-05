using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public Image experienceBar;
    public TextMeshProUGUI level;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void UpdateExperienceBar(float currentExperience, float experienceToNextLevel)
    {
        experienceBar.fillAmount = currentExperience / experienceToNextLevel;
    }

    public void UpdateLevel(int newLevel)
    {
        level.text = newLevel.ToString();
    }
}
