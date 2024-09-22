using UnityEngine;
using System.Collections;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask platformLayer; // Layer for one-way platforms
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private float movementCooldown = 0.2f;
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float dodgeDuration = 0.2f;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool isWallJumping;
    private bool isSliding;
    private bool canMoveAwayFromWall = true;
    private bool isMovementEnabled = true;
    private bool isDodging;
    private float dodgeCooldownTimer = Mathf.Infinity;
    private float originalGravityScale;
    private Collider2D platformCollider; // Collider for one-way platforms

    private void Awake()
    {
        // Getting references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Prevent character from tipping over
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        originalGravityScale = gravityScale;
    }

    private void Update()
    {
        dodgeCooldownTimer += Time.deltaTime;

        // Handle input only if movement is enabled
        if (isMovementEnabled && !isDodging)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            // Flip player code
            if (!isWallJumping && !isSliding && canMoveAwayFromWall)
            {
                if (horizontalInput > 0.01f)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (horizontalInput < -0.01f)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }

            // Set animator parameters
            anim.SetBool("run", horizontalInput != 0);
            anim.SetBool("grounded", isGrounded());

            // Check if the player is falling
            if (!isGrounded() && body.velocity.y < 0 && !onWall())
            {
                anim.SetBool("isFalling", true); // Trigger the falling animation
            }
            else
            {
                anim.SetBool("isFalling", false); // Stop the falling animation
            }

            // Handle Wall Jump and Slide Code
            if (wallJumpCooldown > 0.2f)
            {
                if (onWall() && !isGrounded() && IsMovingTowardsWall())
                {
                    // Slide only when moving towards the wall
                    body.velocity = new Vector2(0, -slideSpeed);
                    body.gravityScale = 0;
                    anim.SetBool("isSliding", true);
                    isSliding = true;

                    // Prevent movement away from the wall while sliding
                    if (horizontalInput * transform.localScale.x < 0)
                    {
                        horizontalInput = 0;
                    }

                    // Reset cooldown to prevent immediate movement after sliding
                    canMoveAwayFromWall = false;
                }
                else
                {
                    // Stop sliding if not moving toward the wall
                    body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
                    body.gravityScale = gravityScale;
                    anim.SetBool("isSliding", false);

                    if (isSliding)
                    {
                        StartCoroutine(MovementCooldown());
                    }

                    isSliding = false;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
            }
            else
            {
                wallJumpCooldown += Time.deltaTime;
            }

            // Handle Dodge Input
            if (Input.GetKeyDown(KeyCode.X) && dodgeCooldownTimer > dodgeCooldown)
            {
                StartCoroutine(Dodge());
            }
        }

        // Ignore platform collisions when jumping upwards
        if (!isGrounded() && body.velocity.y > 0)
        {
            IgnorePlatformCollisions(true);
        }
        else
        {
            IgnorePlatformCollisions(false);
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpForce, jumpHeight);
            wallJumpCooldown = 0;
            isWallJumping = true;
            isSliding = false;
            StartCoroutine(MovementCooldown());

            anim.SetBool("isSliding", false);
            body.gravityScale = gravityScale;
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        dodgeCooldownTimer = 0;
        anim.SetTrigger("dodge");

        float direction = transform.localScale.x;
        float elapsedTime = 0f;

        body.gravityScale = 0;

        while (elapsedTime < dodgeDuration)
        {
            body.velocity = new Vector2(direction * dodgeDistance / dodgeDuration, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        body.velocity = Vector2.zero;
        body.gravityScale = originalGravityScale;
        isDodging = false;
    }

    private IEnumerator MovementCooldown()
    {
        canMoveAwayFromWall = false;
        yield return new WaitForSeconds(movementCooldown);
        canMoveAwayFromWall = true;
        isWallJumping = false;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
 
    public bool IsGrounded()
    {
        return isGrounded();
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private bool IsMovingTowardsWall()
    {
        return (horizontalInput > 0 && transform.localScale.x > 0) || (horizontalInput < 0 && transform.localScale.x < 0);
    }

    public bool canAttack()
    {
        return !isWallJumping && !isSliding && !isDodging;
    }

    public bool IsRunning()
    {
        return horizontalInput != 0 && isGrounded();
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
        body.velocity = Vector2.zero;
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void AddHorizontalMovement(float value)
    {
        body.velocity = new Vector2(value, body.velocity.y);
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    // Method to ignore or re-enable collisions with one-way platforms
    private void IgnorePlatformCollisions(bool ignore)
    {
        if (platformCollider == null || !ignore)
        {
            platformCollider = Physics2D.OverlapBox(transform.position, boxCollider.bounds.size, 0, platformLayer);
        }

        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(boxCollider, platformCollider, ignore);
        }
    }
}
