using System.Collections;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    private float verticalInput;
    private bool isClimbing;
    private Rigidbody2D rb;
    private Animator anim;
    public float climbSpeed = 3f; // Speed of climbing
    public LayerMask ladderLayer; // Layer for ladder detection
    public float animationSpeedFactor = 0.5f; // Factor to control the speed of the animation

    private bool isOnLadder = false; // To check if the player is on the ladder
    private bool isJumpingOffLadder = false; // To prevent double climbing after jump
    private float ladderCooldownTime = 0.2f; // Small delay after jumping off ladder

    private Vector3 originalScale; // Store original player scale for flipping control

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale; // Store the original scale
    }

    private void Update()
    {
        // Player must be on the ladder to initiate climbing
        if (isOnLadder && !isJumpingOffLadder)
        {
            verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput != 0) // Player presses up or down
            {
                StartClimbing();
            }
            else if (isClimbing) // Player is still on the ladder but no vertical input
            {
                PlayClimbIdle();
            }

            // Disable flipping by resetting the scale
            transform.localScale = new Vector3(originalScale.x, transform.localScale.y, transform.localScale.z);
        }

        // Jump off the ladder
        if (Input.GetButtonDown("Jump") && isClimbing)
        {
            JumpOffLadder();
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0f; // Disable gravity while climbing
        rb.velocity = new Vector2(0, verticalInput * climbSpeed); // Move vertically, zero horizontal velocity
       
        // Adjust animation speed based on climbing speed
        float adjustedAnimSpeed = Mathf.Abs(verticalInput) * animationSpeedFactor;
        anim.speed = adjustedAnimSpeed > 0 ? adjustedAnimSpeed : 1f; // Prevent 0 speed for animation
       
        anim.SetBool("isClimbing", true); // Trigger climb animation
        anim.SetFloat("climbSpeed", Mathf.Abs(verticalInput)); // Control speed in animation
    }

    // Play climb idle animation when no vertical input is detected
    private void PlayClimbIdle()
    {
        rb.velocity = Vector2.zero; // Stop all movement, including vertical and horizontal
        anim.speed = 1f; // Reset animation speed to normal when idling
        anim.SetBool("isClimbing", true); // Keep the player in the climbing state
        anim.Play("climbidle"); // Trigger the climb idle animation
    }

    private void JumpOffLadder()
    {
        isClimbing = false;
        rb.gravityScale = 1f; // Restore gravity
        anim.SetBool("isClimbing", false); // Exit climb animation
        rb.velocity = new Vector2(rb.velocity.x, 5f); // Apply upward jump force
        StartCoroutine(LadderCooldown()); // Brief cooldown after jumping
    }

    private IEnumerator LadderCooldown()
    {
        isJumpingOffLadder = true; // Temporarily prevent climbing
        yield return new WaitForSeconds(ladderCooldownTime); // Small delay
        isJumpingOffLadder = false; // Allow climbing again
    }

    // Detect if the player enters the ladder
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true; // Player is on the ladder
        }
    }

    // Detect if the player leaves the ladder
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f; // Restore gravity after leaving ladder
            anim.SetBool("isClimbing", false); // Exit climbing animation
            anim.speed = 1f; // Reset animation speed when leaving ladder

            // Re-enable flipping after leaving the ladder
            transform.localScale = originalScale;
        }
    }
}

