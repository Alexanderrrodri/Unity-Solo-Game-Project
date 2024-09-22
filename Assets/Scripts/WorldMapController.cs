using UnityEngine;
using UnityEngine.SceneManagement; // To load new scenes
using System.Collections;

public class WorldMapController : MonoBehaviour
{
    public Animator miniPlayerAnimator;  // Assign the Animator controlling the mini player's animations
    public float moveSpeed = 5f;         // Speed at which the player moves
    private bool canEnterScene = false;  // Check if the player is in the trigger zone
    private string levelToLoad = "";     // Store the level to load when 'Z' is pressed
    private Vector2 movement;            // Movement direction based on input
    private Rigidbody2D rb;              // Rigidbody for the mini-player
    private bool isEnteringScene = false; // Flag to check if scene transition is happening

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        rb.gravityScale = 0f; // Disable gravity
    }

    private void Update()
    {
        if (!isEnteringScene) // Allow movement only if not transitioning
        {
            HandleMovement();
        }

        // Check if the player can enter a scene and 'Z' is pressed
        if (canEnterScene && Input.GetKeyDown(KeyCode.Z) && !isEnteringScene)
        {
            // Begin the transition process
            isEnteringScene = true;
            miniPlayerAnimator.SetTrigger("Enter"); // Trigger enter animation
        }
    }

    private void HandleMovement()
    {
        // Get input from arrow keys or WASD for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Set movement vector
        movement = new Vector2(moveX, moveY).normalized;

        // If there is movement, play the "minirun" animation
        if (movement != Vector2.zero)
        {
            miniPlayerAnimator.SetBool("isRunning", true); // Play running animation
        }
        else
        {
            miniPlayerAnimator.SetBool("isRunning", false); // Stop running animation
        }

        // Flip the character's sprite depending on the movement direction
        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        if (!isEnteringScene) // Only move if not entering a scene
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // This gets called when the player enters a level's trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level1Trigger")) // For "Level1"
        {
            levelToLoad = "Level1"; // Set the level to load
            canEnterScene = true;
        }
    }

    // This gets called when the player leaves a level's trigger zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Level1Trigger"))
        {
            canEnterScene = false; // Disable the ability to enter the scene
        }
    }

    // This method will be called via an animation event at the end of the "Enter" animation
    public void OnEnterAnimationComplete()
    {
        // Load the specified scene (in this case "Level1")
        SceneManager.LoadScene(levelToLoad);
    }
}
