using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class TopLeftHP : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHpChange += UpdateHealthUI;
            Debug.Log("PlayerStats reference assigned.");
        }
        else
        {
            Debug.LogError("PlayerStats reference not assigned in the Inspector.");
        }

        if (slider != null)
        {
            Debug.Log("Slider reference assigned.");
        }
        else
        {
            Debug.LogError("Slider reference not assigned in the Inspector.");
        }
    }

    private void UpdateHealthUI()
    {
        if (playerStats == null || slider == null)
        {
            Debug.LogError("PlayerStats or Slider reference is null in UpdateHealthUI.");
            return;
        }

        slider.maxValue = playerStats.GetMaxHpValue();
        slider.value = playerStats.currHp;
    }
}
