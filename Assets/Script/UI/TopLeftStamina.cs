using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopLeftStamina : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    private Coroutine staminaCoroutine;

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onStaminaChange += OnStaminaChange;
            UpdateStaminaUI(); // Initial update to set the correct UI values
        }
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.onStaminaChange -= OnStaminaChange;
        }
    }

    private void OnStaminaChange()
    {
        // Stop any existing coroutine to prevent overlapping updates
        if (staminaCoroutine != null)
        {
            StopCoroutine(staminaCoroutine);
        }

        // Start a new coroutine to smoothly update the stamina UI
        staminaCoroutine = StartCoroutine(SmoothStaminaUpdate());
    }

    private IEnumerator SmoothStaminaUpdate()
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration for the smooth transition
        float startValue = slider.value;
        float targetValue = playerStats.currStamina;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        // Ensure the final value is set correctly
        slider.value = targetValue;
    }

    private void UpdateStaminaUI()
    {
        slider.maxValue = playerStats.GetMaxStamina();
        slider.value = playerStats.currStamina;
    }
}
