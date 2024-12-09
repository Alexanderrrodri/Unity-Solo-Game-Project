using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float jumpForce = 5f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject attackHitbox;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private int currentHealth;
    private float attackTimer;
    private bool isGrounded;
    private bool isAttacking = false;

    // Animator Parameters
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int AirSpeed = Animator.StringToHash("AirSpeed");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Attack hitbox not assigned.");
        }
    }

    private void Update()
    {
        if (currentHealth <= 0) return;

        // Ground check using velocity
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.1f;
        anim.SetBool(Grounded, isGrounded);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            EnterCombatIdle();
            HandleMovementAndAttack(distanceToPlayer);
        }
        else
        {
            Patrol();
        }

        anim.SetFloat(AirSpeed, rb.velocity.y);
    }

    private void EnterCombatIdle()
    {
        anim.SetInteger(AnimState, 1);
    }

    private void Patrol()
    {
        anim.SetInteger(AnimState, 2);
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void HandleMovementAndAttack(float distanceToPlayer)
    {
        if (distanceToPlayer > 2f && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        else if (attackTimer >= attackCooldown)
        {
            AttackPlayer();
        }

        if (Random.Range(0, 100) < 10 && isGrounded)
        {
            JumpToDodge();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        anim.SetInteger(AnimState, 2);
        FlipSprite(direction.x);
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        anim.SetTrigger(Attack);
        attackTimer = 0f;
    }

    private void JumpToDodge()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--;
        anim.SetTrigger(Hurt);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger(Death);
        rb.velocity = Vector2.zero;
        enabled = false;
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0 && transform.localScale.x < 0 || direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnAttackStart()
    {
        attackHitbox.SetActive(true);
    }

    private void OnAttackEnd()
    {
        attackHitbox.SetActive(false);
        isAttacking = false;
    }
}