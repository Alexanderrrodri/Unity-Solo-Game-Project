using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For accessing UI components

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    private int score = 0;  // Initial score

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText(); // Display initial score on the screen
    }

    // Method to increase the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText(); // Update the UI text to reflect the new score
    }

    // Method to update the score text
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}