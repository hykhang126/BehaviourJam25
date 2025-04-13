using Unity.VisualScripting;
using UnityEngine;

using Enemies;
using System;

public class Shield : MonoBehaviour
{
    Collider2D sc;

    Animator animator;

    SpriteRenderer spriteRenderer;

    public int playerLayer = 6; // Layer for the player

    public Transform hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = GetComponent<Collider2D>();
        if (sc == null)
        {
            Debug.LogError("Collider2D component not found on the shield object.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the shield object.");
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the shield object.");
        }
        spriteRenderer.enabled = false; // Initially disable the sprite renderer

        // Disable collisions between the shield and player/bullet layers
        Physics2D.IgnoreLayerCollision(playerLayer, sc.gameObject.layer, true); // Ignore shield-player collision
    }

    public void DisableShield()
    {
        sc.enabled = false; // Disable the shield collider
    }

    public void EnableShield()
    {
        if (!spriteRenderer || !sc)
            return;
        if (spriteRenderer.enabled == false)
            spriteRenderer.enabled = true; // Enable the sprite renderer

        sc.enabled = true; // Enable the shield collider
    }

    // Turn off shield sprite
    public void TurnOffShieldSprite()
    {
        if (spriteRenderer)
            spriteRenderer.enabled = false; // Disable the sprite renderer
    }

    // Set trigger for shield animation
    public void SetShieldTrigger(string triggerName)
    {
        if (animator != null && triggerName != null)
        {
            animator.SetTrigger(triggerName); // Set the trigger for the shield animation
        }
    }

    // Set bool for shield animation
    public void SetShieldBool(string value, bool state)
    {
        if (animator != null && value != null)
            animator.SetBool(value, state); // Set the bool for the shield animation
    }

    // Set float for shield animation
    public void SetShieldFloat(string value, float state)
    {
        if (animator != null && value != null)
            animator.SetFloat(value, state); // Set the float for the shield animation
    }
    
    //Collision for static colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10); // Example damage amount
            Debug.Log("Shield hit by enemy: " + collision.gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // Move to parent location
        if (hand != null)
        {
            transform.position = hand.position;
        }
    }
}
