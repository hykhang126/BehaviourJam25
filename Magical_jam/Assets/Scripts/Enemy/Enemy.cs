using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float deathTimer;

    bool death;

    float timer;

    Rigidbody2D rb;

    public GameColor enemyColor;
    
    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        timer = 0;
        death = false;   
    }

    public void Death(){
        rb.freezeRotation = false;
        death = true;
        float timer = deathTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if(death){
            if(timer >= deathTimer){
                Destroy(gameObject);
            }
            timer += Time.deltaTime;
        }

        // set animation based on rb velocity
        if(rb.linearVelocity.x != 0){
            // set animation to walk right or left
            
        } else {
            // set animation to idle
        }
    }
}
