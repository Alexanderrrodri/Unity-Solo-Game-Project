using System.Collections;
using UnityEngine;

public class InvincibilityPowerUp : MonoBehaviour
{
    [SerializeField] private AudioClip invincibilityMusic; // Assign power-up music in Inspector
    [SerializeField] private float invincibilityDuration = 10f;
    public AudioSource bgmSource;
    public AudioSource powerUpSource;
    private bool isInvincible = false;
    private PlayerHealth playerHealth;
    public GameObject powerupArmor;

    private void Awake()
    {
        // Get references to the necessary components
        playerHealth = FindObjectOfType<PlayerHealth>();
        //bgmSource = GameObject.Find("BGMObject").GetComponent<AudioSource>();  
        //powerUpSource = GetComponent<AudioSource>();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player") && !isInvincible)
       {
          // Hide the power-up item immediately
          HidePowerUp();
       
          // Start the invincibility effect
          StartCoroutine(ActivateInvincibility());
        }
    }

    private void HidePowerUp()
    {
       // Disable the visual aspects of the power-up
       GetComponent<SpriteRenderer>().enabled = false;

       // Disable the collider so it can't be triggered again
       GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator ActivateInvincibility()
    {
       isInvincible = true;
       playerHealth.SetInvincible(true); // Make the player invincible

       // Stop background music and play the invincibility music
       bgmSource.Pause();
       powerUpSource.clip = invincibilityMusic;
       powerUpSource.Play();

       // Continue blinking effect or any other visual effect for the player
       StartCoroutine(BlinkEffect());

       // Wait for invincibility duration
       yield return new WaitForSeconds(invincibilityDuration);

       // Stop the power-up music and resume background music
       powerUpSource.Stop();
       bgmSource.UnPause();

       // End invincibility
       playerHealth.SetInvincible(false);
       isInvincible = false;

       // Destroy the power-up object after use
       //Destroy(gameObject);
   }

   // Blinking effect for invincibility
   private IEnumerator BlinkEffect()
   {
       SpriteRenderer spriteRenderer = playerHealth.GetComponent<SpriteRenderer>();
       float blinkInterval = 0.1f;

       for (float i = 0; i < invincibilityDuration; i += blinkInterval)
       {
           spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
           yield return new WaitForSeconds(blinkInterval);
       }

       spriteRenderer.enabled = true; // Ensure sprite is visible after invincibility
   }
}
