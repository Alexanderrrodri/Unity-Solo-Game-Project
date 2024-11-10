using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;     // Assign in the inspector for Game Over text
    public AudioSource bgmSource;            // Reference to background music AudioSource
    public AudioSource gameOverMusicSource;  // AudioSource for Game Over music
    public GameObject gameOverPanel;         // New: Dark transparent panel

    private bool gameIsOver = false;

    private void Start()
    {
        // Ensure the Game Over text and panel are hidden at the start
        gameOverText.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);  // New: Disable panel at start
    }

    private void OnEnable()
    {
        // Subscribe to the player's death event
        FindObjectOfType<PlayerHealth>().OnPlayerDeath += TriggerGameOver;
    }

    private void OnDisable()
    {
        // Unsubscribe when this object is disabled/destroyed
        //FindObjectOfType<PlayerHealth>().OnPlayerDeath -= TriggerGameOver;
    }

    private void TriggerGameOver()
    {

        // Stop background music and play game over music
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        if (gameOverMusicSource != null)
        {
            gameOverMusicSource.Play();
        }

        // Display Game Over text and panel
        gameOverText.gameObject.SetActive(true);
        gameOverPanel.SetActive(true);  // New: Show dark panel

        // Set flag to prevent other inputs
        gameIsOver = true;
    }

    private void Update()
    {
        // Check if the game is over and the Enter key is pressed
        if (gameIsOver && Input.GetKeyDown(KeyCode.Return))
        {

            // Load the WorldMap scene
            SceneManager.LoadScene("WorldMap");
        }
    }
}
