using Unity.VisualScripting;
using UnityEngine;

using Enemies;

public class Shield : MonoBehaviour
{
    Collider2D sc;

    Animator animator;

    SpriteRenderer spriteRenderer;

    public int playerLayer = 6; // Layer for the player

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
        animator.SetTrigger("ShieldOff"); // Trigger the shield off animation
    }

    public void EnableShield()
    {
        if (!spriteRenderer || !sc)
            return;
        if (spriteRenderer.enabled == false)
            spriteRenderer.enabled = true; // Enable the sprite renderer

        sc.enabled = true; // Enable the shield collider
        animator.SetTrigger("ShieldOn"); // Trigger the shield on animation
    }

    // Turn off shield sprite
    public void TurnOffShieldSprite()
    {
        if (spriteRenderer)
            spriteRenderer.enabled = false; // Disable the sprite renderer
    }

    // On collision with enemy, trigger their dameage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(1); // Example damage amount
            Debug.Log("Shield hit by enemy: " + collision.name);
            // Example: collision.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
        else if (collision.CompareTag("Bullet"))
        {
            // Trigger bullet damage logic here
            Debug.Log("Shield hit by bullet: " + collision.name);
            // Example: collision.GetComponent<Bullet>().Destroy();
        }
    }
}
