using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    CircleCollider2D sc;

    float move;

    [SerializeField] float speed;

    int ricochet;

    [SerializeField] int maxRicochet;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sc = GetComponent<CircleCollider2D>();
        ricochet = 0;
        rb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * Time.fixedDeltaTime * speed;
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
