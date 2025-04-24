using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public void setMaxHealth(int playerHealth)
    {
        healthSlider.maxValue = playerHealth;
        healthSlider.value = playerHealth;
    }
    public void setHealth(int playerHealth)
    {
        healthSlider.value = playerHealth;
    }
}
