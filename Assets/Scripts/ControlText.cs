using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    [SerializeField] private GameObject controlTextUI; // Reference to the UI text object

    private bool isTextDismissed = false;

    private void Start()
    {
        // Initially, display the control text when the level starts
        controlTextUI.SetActive(true);

    }

    private void Update()
    {
        // Option to dismiss the text early by pressing any key (e.g., Enter)
        if (!isTextDismissed && Input.GetKeyDown(KeyCode.Return))
        {
            DismissText();
        }
    }


    // Method to hide the control text
    private void DismissText()
    {
        controlTextUI.SetActive(false);
        isTextDismissed = true; // Mark the text as dismissed so it doesn't show again
    }
}
