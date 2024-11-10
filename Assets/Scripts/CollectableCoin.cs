using System.Collections;
using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    public AudioClip collectSound;    // Assign the sound to play when collected
    private AudioSource audioSource;  // AudioSource component
    private bool collected = false;   // Ensure the coin is only collected once

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;  // Prevent multiple collections
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // Play the collect sound at the coin's position without needing the coin object to remain
        AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Increase the player's score
        FindObjectOfType<ScoreManager>().IncreaseScore(100);

        // Destroy the coin immediately
        Destroy(gameObject);
    }
}
