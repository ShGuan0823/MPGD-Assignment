using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    Image healthSlider;

    Image energySlider;

    private void Awake()
    {
        healthSlider = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        energySlider = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateEnergy();
    }

    private void UpdateHealth()
    {
        //float healthPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        //healthSlider.fillAmount = healthPercent;
    }

    private void UpdateEnergy()
    {
        //float energyPercent = (float)GameManager.Instance.playerStats.CurrentEnergy / GameManager.Instance.playerStats.MaxEnergy;
        //energySlider.fillAmount = energyPercent;
    }
}
