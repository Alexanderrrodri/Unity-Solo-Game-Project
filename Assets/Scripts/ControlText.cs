using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    [SerializeField] private GameObject controlTextUI; // Reference to the UI text object
    [SerializeField] private float displayDuration = 5f; // Duration to display the text before hiding
    private bool isTextDismissed = false;

    private void Start()
    {
        // Initially, display the control text when the level starts
        controlTextUI.SetActive(true);

        // Hide the text automatically after a few seconds or when the player presses a key
        StartCoroutine(HideControlTextAfterDelay());
    }

    private void Update()
    {
        // Option to dismiss the text early by pressing any key (e.g., Enter)
        if (!isTextDismissed && Input.GetKeyDown(KeyCode.Return))
        {
            DismissText();
        }
    }

    // Hide the text after the display duration
    private IEnumerator HideControlTextAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        DismissText();
    }

    // Method to hide the control text
    private void DismissText()
    {
        controlTextUI.SetActive(false);
        isTextDismissed = true; // Mark the text as dismissed so it doesn't show again
    }
}
