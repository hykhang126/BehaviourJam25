using UnityEngine;

using Characters;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Shield : MonoBehaviour
{
    private Collider2D sc;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    [Header("Shield Setup")]
    public int playerLayer = 6; // Layer for the player

    [Header("Shield Stats")]
    public float shieldDamage = 10f;

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

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the shield object.");
        }

        // Setup shield
        InitializeShield();
    }

    private void InitializeShield()
    {
        // Disable collisions between the shield and player/bullet layers
        Physics2D.IgnoreLayerCollision(playerLayer, sc.gameObject.layer, true);

        // Set shield rigidbody to be kinematic and locked position
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void ToggleShield(bool toggle)
    {
        if (toggle)
        {
            EnableShield();
            TurnOnShieldSprite();
        }
        else
        {
            DisableShield();
            TurnOffShieldSprite();
        }
    }

    public void DisableShield()
    {
        if (!sc)
        {
            return;
        }

        sc.enabled = false; // Disable the shield collider
    }

    public void EnableShield()
    {
        if (!sc)
        {
            return;
        }

        sc.enabled = true; // Enable the shield collider
    }

    // Turn off shield sprite
    public void TurnOffShieldSprite()
    {
        if (spriteRenderer)
        {
            spriteRenderer.enabled = false; // Disable the sprite renderer
        }
    }

    // Turn off shield sprite
    public void TurnOnShieldSprite()
    {
        if (spriteRenderer)
        {
            spriteRenderer.enabled = true; // Enable the sprite renderer
        }
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
        {
            animator.SetBool(value, state); // Set the bool for the shield animation
        }
    }

    // Set float for shield animation
    public void SetShieldFloat(string value, float state)
    {
        if (animator != null && value != null)
        {
            animator.SetFloat(value, state); // Set the float for the shield animation
        }
    }

    //Collision for static colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    void Update()
    {
    }
}
