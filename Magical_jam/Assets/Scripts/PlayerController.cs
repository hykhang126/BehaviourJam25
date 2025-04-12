using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


using Combat;
 
public class PlayerController : MonoBehaviour
{
    Player player;
    PlayerControls controls;
    Vector2 move;
    Rigidbody2D rb;
    Animator animator;
    Melee melee;
    public float speed = 10f;
    public float dashSpeed = 2f;
    public float dashCooldown = 3f; // Cooldown time between dashes in seconds
    private float dashTime;
    private Vector2 dashVector = Vector2.zero; // Vector to store the dash direction

    void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found on the player object.");
        }

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

        melee = GetComponentInChildren<Melee>();
        if (melee == null)
        {
            Debug.LogError("Melee component not found in the children of the player object.");
        }

        // Movement
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>().normalized;
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        // Dash
        controls.Player.Dash.performed += ctx => Dash();

        // Action (Fire)
        controls.Player.Fire.performed += ctx => FireAction();
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
            return; // Exit the method if dash is on cooldown
        }
        dashTime = dashCooldown; // Reset the cooldown timer
        animator.SetBool("Dashing", true); // Trigger the dash animation
        player.SetIsHit(true); // Set the hit state to true
        Vector2 dir = move.normalized;
        dashVector = dir * dashSpeed;
    }

    private void HandleShoot()
    {
        player.TryShoot(Input.mousePosition);
    }

    // Action depending on the color
    void FireAction()
    {
        // More logic depending on the color
        switch (player.GetPlayerColor())
        {
            case LevelColor.Red:
                HandleShoot();
                break;
            case LevelColor.Blue:
                // Perform action for blue color
                break;
            case LevelColor.Green:
                melee.Attack();
                
                Debug.Log("Melee performed! " + GetType());
                break;
            default:
                Debug.Log("No action defined for this color.");
                break;
        }
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
        if (dashVector.magnitude <= 0.1f)
        {
            animator.SetBool("Dashing", false); // Reset the dash animation when not dashing
            player.SetIsHit(false); // Reset the hit state when not dashing
        }
        // Update the speed parameter in the animator
        animator.SetFloat("Speed", move.magnitude); // Set the speed parameter based on movement input
    }
}