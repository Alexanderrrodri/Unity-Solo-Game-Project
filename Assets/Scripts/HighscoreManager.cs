using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour
{
    private int highscore = 0;
    private string highscoreKey;

    private void Start()
    {
        // Determine the highscore key based on the current level
        string currentLevel = SceneManager.GetActiveScene().name;
        highscoreKey = currentLevel + "Highscore"; // Example: "Level1Highscore", "Level2Highscore"

        // Load the saved highscore (if any) from PlayerPrefs
        highscore = PlayerPrefs.GetInt(highscoreKey, 0); // Default value is 0
    }

    // Method to update the highscore if the current score is higher
    public void CheckAndSetHighscore(int currentScore)
    {
        if (currentScore > highscore)
        {
            highscore = currentScore;
            PlayerPrefs.SetInt(highscoreKey, highscore); // Save the new highscore
            PlayerPrefs.Save();
        }
    }

    // Method to get the current highscore
    public int GetHighscore()
    {
        return highscore;
    }
}
