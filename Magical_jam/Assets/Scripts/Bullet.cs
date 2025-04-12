using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int maxRicochet;
    
    // Components
    Rigidbody2D rb;
    CircleCollider2D sc;
    
    // Data
    private Vector3 trajectory;
    private int ricochet;

    public void Initialize(Vector3 trajectory)
    {
        this.trajectory = trajectory;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Death();
            Destroy(gameObject);
        }
        
        if (ricochet == maxRicochet)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
            }
            else if (Math.Abs(rb.linearVelocity.x) < 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180 - transform.rotation.eulerAngles.z);
            }
            
            ricochet++;
        }
    }
}
