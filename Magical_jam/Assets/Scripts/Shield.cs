using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D sc;

    public LayerMask playerLayer; // Layer for the player
    public LayerMask bulletLayer; // Layer for the bullets

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = GetComponent<Collider2D>();
        if (sc == null)
        {
            Debug.LogError("Collider2D component not found on the shield object.");
        }

        // Disable collisions between the shield and player/bullet layers
        Physics2D.IgnoreLayerCollision(playerLayer, sc.gameObject.layer, true); // Ignore shield-player collision
        Physics2D.IgnoreLayerCollision(bulletLayer, sc.gameObject.layer, true); // Ignore shield-bullet collision
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
