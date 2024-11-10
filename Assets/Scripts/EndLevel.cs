using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;       // Assign a Text UI element to display the highscore
    public HighscoreManager highscoreManager;   // Reference to the HighscoreManager
    public ScoreManager scoreManager;           // Reference to the ScoreManager to get the current score
    public AudioSource bgmSource;               // Reference to background music AudioSource
    public AudioSource victoryMusicSource;      // AudioSource for victory music
    public TimerManager timerManager;           // Reference to TimerManager to get the remaining time
    private bool highscoreDisplayed = false;    // Track if the highscore is currently displayed


    private void Start()
    {
        // Make sure the TimerManager is referenced
        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TimerManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Stop the timer and calculate remaining time as score
            int remainingTimeBonus = timerManager.EndLevel(); // Stops timer and returns the remaining time bonus (5 points per second)
            scoreManager.IncreaseScore(remainingTimeBonus);   // Add time bonus to the score

            // Get the player's current score
            int currentScore = scoreManager.GetScore();

            // Check and update highscore
            highscoreManager.CheckAndSetHighscore(currentScore);

            // Display the highscore on the UI and enable the text
            highscoreText.text = "Press Enter to close window" + "\n Highscore: " + highscoreManager.GetHighscore().ToString();
            highscoreText.gameObject.SetActive(true);  // Show the highscore text

            // Stop background music and play victory music
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }

            if (victoryMusicSource != null)
            {
                victoryMusicSource.Play();
            }

            // Freeze the game
            Time.timeScale = 0f;

            // Set the highscoreDisplayed flag to true so we can check for "Enter" press
            highscoreDisplayed = true;
        }
    }

    private void Update()
    {
        // Check if highscore is displayed and if the player presses "Enter"
        if (highscoreDisplayed && Input.GetKeyDown(KeyCode.Return)) // "Return" is the Enter key
        {
            // Unfreeze the game before loading the new scene
            Time.timeScale = 1f;

            // Load the WorldMap scene
            SceneManager.LoadScene("WorldMap");
        }
    }
}