using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameSelect : MonoBehaviour
{
    // Method to load the Level1 scene
    public void StartNewGame()
    {
        // Load the Level1 scene
        SceneManager.LoadScene("WorldMap");
    }
}