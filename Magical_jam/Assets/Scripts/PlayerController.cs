using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    Rigidbody2D rb;
    Animator animator;
    public float speed = 10f;
    public float dashSpeed = 20f;
    public float dashCooldown = 3f; // Cooldown time between dashes in seconds
    private float dashTime = 0f;
    private Vector2 dashVector = Vector2.zero; // Vector to store the dash direction

    void Awake()
    {
        controls = new PlayerControls();

        dashTime = 0.0f; // Initialize the cooldown timer

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }

        // Movement
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>().normalized;
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        // Dash
        controls.Player.Dash.performed += ctx => Dash();
    }
    
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    // Dash
    void Dash()
    {
        if (dashTime > 0f || move == Vector2.zero) // Check if dash is on cooldown
        {
            Debug.Log("Dash is on cooldown or no movement input detected.");
            return; // Exit the method if dash is on cooldown
        }
        dashTime = dashCooldown; // Reset the cooldown timer
        animator.SetBool("Dashing", true); // Trigger the dash animation
        Vector2 dir = move.normalized;
        dashVector = dir * dashSpeed;
    }
    
    void FixedUpdate()
    {
        Vector2 movement = speed * Time.deltaTime * new Vector2(move.x, move.y) + dashVector;
        transform.Translate(movement, Space.World);

        // Update the dash cooldown timer
        if (dashTime > 0f)
        {
            dashTime -= Time.fixedDeltaTime;
            dashVector = Vector2.Lerp(dashVector, Vector2.zero, Time.fixedDeltaTime * 5f); // Gradually reduce the dash vector to zero
        }
        else
        {
            dashVector = Vector2.zero; // Reset the dash vector when cooldown is over
        }

        // Update the animator parameters based on movement
        if (dashVector.magnitude <= 0.5f)
        {
            animator.SetBool("Dashing", false); // Reset the dash animation when not dashing
        }
        // Update the speed parameter in the animator
        animator.SetFloat("Speed", move.magnitude); // Set the speed parameter based on movement input
    }
}