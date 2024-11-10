using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Reference to the Pause Menu panel
    [SerializeField] private GameObject controlTextUI; // Reference to the Control Text UI
    [SerializeField] private GameObject worldMapButton;

    private bool isPaused = false; // To track the game's pause state
    private bool isShowingControls = false; // To track if control text is being shown

    private void Update()
    {
        // Check for the 'M' key press to toggle the pause state
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isShowingControls)
            {
                // Close the control menu when the pause button is pressed
                CloseControlMenu();
            }
            else
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        // Check if Enter is pressed while showing controls
        if (isShowingControls && Input.GetKeyDown(KeyCode.Return))
        {
            CloseControlMenu();
        }
    }

    // Method to pause the game
    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true); // Show the pause menu
        worldMapButton.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
    }

    // Method to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        worldMapButton.SetActive(false);
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Unfreeze the game
    }

    // Method to show the control menu
    public void ShowControlMenu()
    {
        worldMapButton.SetActive(false);
        isShowingControls = true;
        controlTextUI.SetActive(true); // Show the control text UI
        pauseMenu.SetActive(false); // Hide the pause menu
    }

    // Method to close the control menu and return to the pause menu
    public void CloseControlMenu()
    {
        isShowingControls = false;
        controlTextUI.SetActive(false); // Hide the control text UI
        pauseMenu.SetActive(true); // Show the pause menu again
        worldMapButton.SetActive(true);
    }

    // Method to return to the title screen
    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // Ensure the game is unpaused before switching scenes
        SceneManager.LoadScene("TitleScreen"); // Change "TitleScreen" to the actual name of your title screen scene
    }

    //Method to return to World Map
    public void ReturnToWorldMap()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("WorldMap"); 
    }

}