using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float dashSlideDuration = 0.5f;
    [SerializeField] private float dashSlideSpeed = 2f;
    [SerializeField] private float slightForwardMovementDistance = 0.1f; // Small movement distance
    [SerializeField] private GameObject attackHitbox; // Reference to the attack hitbox GameObject

    private Animator anim;
    private MovementPlayer movementPlayer;
    private PlayerHealth playerHealth; // Reference to PlayerHealth
    private float cooldownTimer = Mathf.Infinity;
    private bool isDashing;
    private bool isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movementPlayer = GetComponent<MovementPlayer>();
        playerHealth = GetComponent<PlayerHealth>(); // Get PlayerHealth component
        attackHitbox.SetActive(false); // Ensure hitbox is disabled initially
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Check if the player is dead before allowing attacks
        if (playerHealth != null && playerHealth.IsDead()) return;

        // Check for attack input and that cooldown has elapsed
        if (Input.GetKeyDown(KeyCode.Z) && cooldownTimer > attackCooldown)
        {
            if (movementPlayer.IsRunning() && !isDashing && !isAttacking)
            {
                DashAttack();
            }
            else if (movementPlayer.canAttack() && !isDashing && !isAttacking)
            {
                NormalAttack(); // Trigger the attack animation
            }
        }
    }

    private void NormalAttack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        isAttacking = true;

        // Check if the player is in the air
        if (movementPlayer.IsGrounded())
        {
            // Player is on the ground, so apply slight forward movement
            StartCoroutine(EndAttackWithMovement());
        }
        else
        {
            // Player is in the air, no forward movement
            StartCoroutine(EndAttackk());
        }
    }

    private void DashAttack()
    {
        anim.SetTrigger("dashAttack");
        cooldownTimer = 0;
        isDashing = true;
        movementPlayer.DisableMovement();
        StartCoroutine(SlideAfterDash());
    }

    private IEnumerator SlideAfterDash()
    {
        float elapsedTime = 0f;
        while (elapsedTime < dashSlideDuration)
        {
            float slideSpeed = Mathf.Lerp(dashSlideSpeed, 0, elapsedTime / dashSlideDuration);
            movementPlayer.AddHorizontalMovement(slideSpeed * Mathf.Sign(movementPlayer.GetHorizontalInput()));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        movementPlayer.EnableMovement();
        isDashing = false;
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }

    private IEnumerator EndAttackWithMovement()
    {
        float attackDuration = 0.5f; // Adjust this duration to match your animation length
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            // Apply slight forward movement
            movementPlayer.AddHorizontalMovement(slightForwardMovementDistance / attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset attack state after the attack duration
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }

    private IEnumerator EndAttackk()
    {
        float attackDuration = 0.5f; // Adjust this duration to match your animation length
        yield return new WaitForSeconds(attackDuration);

        // Reset attack state after the attack duration
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }

    // Animation Event method to activate hitbox during the attack
    public void EnableHitbox()
    {
        attackHitbox.SetActive(true);
    }

    // Animation Event method to deactivate hitbox after the attack
    public void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }

    public void EndDashAttack()
    {
        // Empty method, as the coroutine now handles the end of dash attack
    }
}
