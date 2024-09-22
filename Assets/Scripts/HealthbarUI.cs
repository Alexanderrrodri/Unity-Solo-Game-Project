using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthbarTotal;   // Reference to the black hearts
    [SerializeField] private Image healthbarCurrent; // Reference to the red hearts
    [SerializeField] private PlayerHealth playerHealth; // Reference to the PlayerHealth script

    private void Start()
    {
        // Set initial health bar state
        InitializeHealthBar();

        // Subscribe to the OnHealthChanged event from the PlayerHealth script
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void InitializeHealthBar()
    {
        // Initialize the health bar states
        healthbarTotal.fillAmount = 0.298f;   // Set to the visual amount representing 3 black hearts
        healthbarCurrent.fillAmount = 0.298f; // Set to match the starting max health of 3 red hearts
    }

    private void UpdateHealthBar()
    {
        // Get the current and max health from the PlayerHealth script
        int currentHealth = playerHealth.GetCurrentHealth();
        int maxHealth = playerHealth.GetMaxHealth();

        // Calculate the fill amount based on current health and max health ratio
        float fillAmount = 0.298f * ((float)currentHealth / maxHealth);

        // Update the fill amount of the current health bar
        healthbarCurrent.fillAmount = fillAmount;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this script is destroyed
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
