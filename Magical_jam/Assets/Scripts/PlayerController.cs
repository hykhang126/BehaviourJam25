using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


using Combat;
using Levels;

public class PlayerController : MonoBehaviour
{
    Player player;
    PlayerControls controls;
    Vector2 move;
    Rigidbody2D rb;
    Animator animator;
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

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player's child object.");
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
        player.SetShieldBool("Dashing", true);
        player.SetIsInvincible(true); // Set the hit state to true
        player.SetBunVisibility(false);
        Vector2 dir = move.normalized;
        dashVector = dir * dashSpeed;
    }

    // Action depending on the color
    void FireAction()
    {
        if (player) player.FireAction();
    }
    
    void FixedUpdate()
    {
        Vector2 movement = speed * Time.deltaTime * new Vector2(move.x, move.y) + dashVector;
        // clamp postion to -100 to 100
        if (transform.position.x < -100 || transform.position.x > 100 || transform.position.y < -100 || transform.position.y > 100)
        {
            movement = Vector2.zero; // Stop the player from moving outside the bounds
        }
        transform.Translate(movement, Space.World);

        // Update the dash cooldown timer
        if (dashTime > 0f)
        {
            dashTime -= Time.fixedDeltaTime;
            dashVector = Vector2.Lerp(dashVector, Vector2.zero, Time.fixedDeltaTime * 12f); // Gradually reduce the dash vector to zero
        }
        else
        {
            dashVector = Vector2.zero; // Reset the dash vector when cooldown is over
        }

        // Update the animator parameters based on movement
        if (dashVector.magnitude <= 0.01f)
        {
            animator.SetBool("Dashing", false); // Reset the dash animation when not dashing
            player.SetShieldBool("Dashing", false); // Reset the shield trigger when not dashing
            player.SetIsInvincible(false); // Reset the hit state when not dashing
            player.SetBunVisibility(true); // Show the bun when not dashing
        }
        // Update the speed parameter in the animator
        animator.SetFloat("Speed", move.magnitude); // Set the speed parameter based on movement input
        player.SetPlayerSpeed(move.magnitude); // Set the player's speed based on movement input
    }
}