using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float knockbackDistance = 2f;

    private int currentHealth;
    private bool isInvincible = false;
    private bool isDead = false; // Added flag to prevent actions when dead
    private Rigidbody2D rb;
    private Animator anim;

    // Declare an event for health changes and game over
    public event Action OnHealthChanged;
    public event Action OnPlayerDeath;  // New event for when the player dies


    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Check if the Animator is attached
        if (anim == null)
        {
            Debug.LogError("Animator component not found on the player! Please attach an Animator.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHitbox") && !isInvincible && !isDead)
        {
            // Defaulting to false since we assume it's a normal attack if not specified.
            TakeDamage(false);
        }
    }

    public void TakeDamage(bool isDashAttack = false)
    {
        if (isInvincible || isDead) return;

        currentHealth--;

        // Check if anim is assigned before triggering the animation
        if (anim != null)
        {
            anim.SetTrigger("takeDamage");
        }

        // Apply knockback based on attack type
        Vector2 knockbackDirection = (transform.position - GameObject.FindGameObjectWithTag("Enemy").transform.position).normalized;
        rb.velocity = knockbackDirection * knockbackDistance;

        // Trigger the health update
        OnHealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(StartInvincibility());
        }
    }

    public void ForceDeath()
    {
        if (isDead) return; // Prevent death handling from triggering again
        currentHealth = 0;  // Set health to 0
        OnHealthChanged?.Invoke(); // Trigger health update
        Die();  // Trigger death sequence
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // New method to get the max health value
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private IEnumerator StartInvincibility()
    {
        isInvincible = true;

        // Make the player blink to indicate invincibility
        float blinkInterval = 0.1f; // Adjust this value for blink speed
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (float i = 0; i < invincibilityDuration; i += blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(blinkInterval);
        }

        spriteRenderer.enabled = true; // Ensure the sprite is visible after blinking ends
        isInvincible = false;
    }

    private void Die()
    {
        if (isDead) return; // Prevent death handling from triggering again
        isDead = true; // Set the dead flag to true

        // Trigger death animation
        if (anim != null)
        {
            // Disable player movement and interaction
            GetComponent<MovementPlayer>().DisableMovement();
            anim.SetTrigger("die");
        }
        // Trigger the Game Over screen
        OnPlayerDeath?.Invoke();
    }

    // Method to restore health and trigger health update
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetInvincible(bool value)
    {
       isInvincible = value;
    }
}
