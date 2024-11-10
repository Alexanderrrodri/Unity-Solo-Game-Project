using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject door; // The door object to toggle visibility
    [SerializeField] private AudioClip plateSound; // Sound to play when activating the plate
    [SerializeField] private bool isActive = false; // Tracks whether the plate is currently activated

    private AudioSource audioSource;
    private int objectCount = 0; // Counts objects on the plate

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the door reference is set
        if (door == null)
        {
            Debug.LogWarning("Door not assigned in PressurePlate script.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if an object enters the plate
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("MovableObject"))
        {
            objectCount++;
            ActivatePressurePlate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if an object leaves the plate
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("MovableObject"))
        {
            objectCount--;
            if (objectCount <= 0)
            {
                DeactivatePressurePlate();
            }
        }
    }

    private void ActivatePressurePlate()
    {
        if (!isActive)
        {
            isActive = true;
            ToggleDoor(false); // Hides the door
            PlaySound();
        }
    }

    private void DeactivatePressurePlate()
    {
        if (isActive)
        {
            isActive = false;
            ToggleDoor(true); // Makes the door reappear
            PlaySound();
        }
    }

    private void ToggleDoor(bool state)
    {
        if (door != null)
        {
            door.SetActive(state);
        }
    }

    private void PlaySound()
    {
        if (plateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(plateSound);
        }
    }
}