using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public Image experienceBar;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
