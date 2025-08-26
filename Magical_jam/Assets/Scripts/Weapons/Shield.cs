using UnityEngine;

using Characters;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Shield : Weapon
{
    private Collider2D sc;

    private Rigidbody2D rb;

    [Header("Shield Setup")]
    public int playerLayer = 6; // Layer for the player

    [Header("Shield Stats")]
    public float shieldDamage = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        sc = GetComponent<Collider2D>();
        if (sc == null)
        {
            Debug.LogError("Collider2D component not found on the shield object.");
        }

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
            EnableSprite();
        }
        else
        {
            DisableShield();
            DisableSprite();
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

    //Collision for colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
        }
    }
}
