using System;
using UnityEngine;
using Enemies;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private int maxRicochet;

    [SerializeField] private bool shotByPlayer = true;
    
    // Components
    Rigidbody2D rb;
    CircleCollider2D sc;
    
    // Data
    private Vector3 trajectory;
    private int ricochet;

    public void Initialize(Vector3 trajectory, string bulletOwner, float speed = 2000f, float damage = 10f)
    {
        if (bulletOwner.Contains("Player"))
        {
            shotByPlayer = true;
        }
        else if (bulletOwner.Contains("Enemy"))
        {
            shotByPlayer = false;
        }
        
        // Set the trajectory of the bullet
        this.trajectory = trajectory.normalized;

        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        sc = GetComponent<CircleCollider2D>();
        ricochet = 0;
        rb.linearVelocity = Vector2.zero;

        // Set the bullet to be destroyed after 5 seconds
        Destroy(gameObject, 5f);
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = trajectory * (Time.fixedDeltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && shotByPlayer)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }

        // Player damage
        if (collision.gameObject.CompareTag("Player") && !shotByPlayer)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }

        // if hit shield, change shotByPlayer to true
        if (collision.gameObject.CompareTag("Shield") && !shotByPlayer)
        {
            shotByPlayer = true;
            // Player shield hit sfx
        }
        
        // Ricochet
        if (ricochet == maxRicochet)
        {
            Destroy(gameObject);
        }
        else
        {
            // Ricochet opposite way with a small angle deviation
            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectDir = Vector2.Reflect(trajectory, normal);
            float angle = UnityEngine.Random.Range(-10f, 10f);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            trajectory = rotation * reflectDir;
            rb.linearVelocity = trajectory * speed;
            
            ricochet++;
        }
    }
}
