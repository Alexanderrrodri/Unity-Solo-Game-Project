using System.Collections;
using UnityEngine;

public class ObjectD : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1; // Number of hits to destroy
    [SerializeField] private AudioClip destroySound; // Sound to play when destroyed
    [SerializeField] private float destroyDelay = 0.2f; // Optional delay before destroying object
    [SerializeField] private ScoreManager scoreManager; // Optional: for adding points when destroyed

    private AudioSource audioSource;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // This function is called when the player attacks the obstacle
    public void TakeHit()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            DestroyObstacle();
        }
    }

    private void DestroyObstacle()
    {
        // Play destroy sound
        if (destroySound != null)
        {
            audioSource.PlayOneShot(destroySound);
        }

        // Optional: Add points to score when obstacle is destroyed
        if (scoreManager != null)
        {
            scoreManager.IncreaseScore(100);
        }

        // Disable the collider to prevent further interaction
        Collider2D obstacleCollider = GetComponent<Collider2D>();
        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = false;
        }

        // Optionally delay destruction to allow sound to play
        Destroy(gameObject, destroyDelay);
    }
}
