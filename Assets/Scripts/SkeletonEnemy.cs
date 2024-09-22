using UnityEngine;
using System.Collections;

public class SkeletonEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private float despawnTime = 2f;
    [SerializeField] private float invincibilityDuration = 0.5f;
    [SerializeField] private float knockbackDistanceNormal = 2f;
    [SerializeField] private float knockbackDistanceDash = 4f;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackMoveDistance = 0.2f; // Distance to move during attack
    [SerializeField] private float pushForce = 5f; // Force to push the player during skelshield

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private Collider2D skeletonCollider;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private float randomMoveTimer = 0f;
    private int randomDirection = 1;
    private int currentHealth;
    private bool isDying = false;
    private bool isInvincible = false;
    private bool isHitByDashAttack = false;
    private bool isInvincibleDuringShield = false; // Added flag for invincibility during shield move

    public ScoreManager scoreManager; // Reference to the ScoreManager

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        skeletonCollider = GetComponent<Collider2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 5f;
        currentHealth = maxHealth;

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Attack hitbox not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (isDying) return;

        attackTimer += Time.deltaTime;
        randomMoveTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (attackTimer >= attackCooldown)
                {
                    AttackPlayer();
                }
            }
        }
        else
        {
            if (randomMoveTimer >= 1f)
            {
                RandomWalk();
                randomMoveTimer = 0f;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("isMoving", false);
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        anim.SetBool("isMoving", true);
        transform.localScale = new Vector3(Mathf.Sign(direction.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void AttackPlayer()
    {
        rb.velocity = Vector2.zero;
        isAttacking = true;
        attackTimer = 0f;

        int attackChoice = Random.Range(0, 3); // Adding a third option for shield push
        if (attackChoice == 0)
        {
            anim.Play("skelattack");
        }
        else if (attackChoice == 1)
        {
            anim.Play("skelattack2");
        }
        else
        {
            anim.Play("skelshield"); // Play the shield move animation
        }
    }

    private void RandomWalk()
    {
        if (!isAttacking)
        {
            randomDirection = Random.Range(-1, 2);
            rb.velocity = new Vector2(randomDirection * speed, rb.velocity.y);
            anim.SetBool("isMoving", randomDirection != 0);

            if (randomDirection != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    // Method to push the player during the skelshield animation
    public void PushPlayer()
    {
        if (player != null)
        {
            Vector2 pushDirection = (player.position - transform.position).normalized;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    public void TakeHit(bool isDashAttack)
    {
        if (isDying || isInvincible || isInvincibleDuringShield) return; // Ignore hits during shield invincibility

        currentHealth--;
        anim.SetTrigger("takehit");

        if (currentHealth <= 0)
        {
            scoreManager.IncreaseScore(100);
            Die();
        }
        else
        {
            isHitByDashAttack = isDashAttack;
            StartCoroutine(StartInvincibility());
        }
    }

    private void Die()
    {
        isDying = true;
        anim.SetTrigger("die");
        anim.SetBool("isMoving", false);
        rb.velocity = Vector2.zero;
        DisableCollision();
        Invoke(nameof(RemoveSkeleton), despawnTime);
    }

    private void DisableCollision()
    {
        if (skeletonCollider != null)
        {
            skeletonCollider.enabled = false;
        }
    }

    private void RemoveSkeleton()
    {
        Destroy(gameObject);
    }

    private void StartAttack()
    {
        attackHitbox.SetActive(true);
    }

    private void EndAttack()
    {
        attackHitbox.SetActive(false);
    }

    // Animation Event Method to move the skeleton slightly during attack
    public void MoveDuringAttack()
    {
        Vector2 attackDirection = new Vector2(transform.localScale.x * attackMoveDistance, 0);
        rb.MovePosition(rb.position + attackDirection);
    }

    private IEnumerator StartInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        float knockbackDistance = isHitByDashAttack ? knockbackDistanceDash : knockbackDistanceNormal;
        isHitByDashAttack = false;

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.velocity = knockbackDirection * knockbackDistance;
    }

    // Added methods to manage invincibility during the shield move

    // Start of the shield move (triggered by animation event)
    public void StartShieldMove()
    {
        isInvincibleDuringShield = true; // Enable invincibility during shield
    }

    // End of the shield move (triggered by animation event)
    public void EndShieldMove()
    {
        isInvincibleDuringShield = false; // Disable invincibility after shield move
    }
}
