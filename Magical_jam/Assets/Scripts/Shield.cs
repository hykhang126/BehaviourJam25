using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D sc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = GetComponent<Collider2D>();
        if (sc == null)
        {
            Debug.LogError("Collider2D component not found on the shield object.");
        }

        // SetApplicationVariable this collider to ignore player collisions
        Physics2D.IgnoreCollision(sc, GetComponentInParent<Collider2D>(), true);
    }

    public void DisableShield()
    {
        sc.enabled = false; // Disable the shield collider
    }

    public void EnableShield()
    {
        sc.enabled = true; // Enable the shield collider
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
