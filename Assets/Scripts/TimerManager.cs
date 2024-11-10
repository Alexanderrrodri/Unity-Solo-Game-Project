using System.Collections;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private int startTime = 900; // Start time set to 900 seconds
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the timer UI
    private int currentTime;
    private bool isTimerRunning = true;
    private PlayerHealth playerHealth; // Reference to PlayerHealth

    private void Start()
    {
        currentTime = startTime; // Set the current time to the start time
        playerHealth = FindObjectOfType<PlayerHealth>(); // Get the PlayerHealth component
        UpdateTimerText(); // Update the timer display initially
        StartCoroutine(CountdownTimer()); // Start the countdown
    }

    private IEnumerator CountdownTimer()
    {
        while (currentTime > 0 && isTimerRunning)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            currentTime--; // Decrease the time by 1 second
            UpdateTimerText(); // Update the displayed timer
        }

        if (currentTime <= 0 && isTimerRunning)
        {
            // Timer has run out, stop it
            isTimerRunning = false;
           
            // Trigger player death due to time running out
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.ForceDeath(); // Force player's health to 0 and trigger death            }
            }
        }
    }

    private void UpdateTimerText()
    {
        // Convert time to minutes and seconds format for display
        int minutes = currentTime / 60;
        int seconds = currentTime % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Call this method when the player reaches the end of the level
    public int EndLevel()
    {
        isTimerRunning = false; // Stop the timer

        // Calculate score from remaining time (5 points per second)
        int timeBonus = currentTime * 5;

        // Optionally reset the timer for the next level if needed
        currentTime = 0;

        return timeBonus; // Return the calculated time bonus
    }
}

