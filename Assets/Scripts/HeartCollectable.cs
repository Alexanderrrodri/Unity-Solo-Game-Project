using System.Collections;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound; // Sound to play when collected
    [SerializeField] private int healthAmount = 1; // Amount of health to restore
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Access PlayerHealth script and restore health
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.Heal(healthAmount);
               
                // Play the collect sound
                if (collectSound != null)
                {
                    audioSource.PlayOneShot(collectSound);
                }

                // Hide the heart object (make it disappear)
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                // Destroy the heart after sound finishes playing
                Destroy(gameObject, collectSound.length);
            }
        }
    }
}
