using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    // Method to load the Level1 scene
    public void StartCredits()
    {
        // Load the Level1 scene
        SceneManager.LoadScene("Credits");
    }
}
