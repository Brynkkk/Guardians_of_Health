using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;
    private CharacterStats stats;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();


        entity.onFlipped += FlipUI;
    }

    private void Update()
    {
        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        slider.maxValue = stats.GetMaxStamina();
        slider.value = stats.currStamina;
    }

    private void FlipUI()
    {
        rectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
    }
}
