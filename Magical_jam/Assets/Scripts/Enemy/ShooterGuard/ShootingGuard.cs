using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGuard : StateManager
{
    [SerializeField] private float playerInView;
    [SerializeField] private float playerInRange;
    [SerializeField] private float speed;
    [SerializeField] private EnemyGun gun;
    private Player player;
    private float playerDistance;
    
    private bool facingRight = true;
    
    public void Start()
    {
        player = GameManager.Instance.player;
        CurrentState = new IdleGuardState(this);
        CurrentState.Start();

        
        

        
    }

    public bool isPlayerNearby()
    {
        playerDistance = player.transform.position.x - transform.position.x;
        
        return (Mathf.Abs(playerDistance) < playerInView);
    }

    public bool isNextToPlayer()
    {
        playerDistance = player.transform.position.x - transform.position.x;
        return (Mathf.Abs(playerDistance) < playerInRange);
    }

    public void moveToPlayer()
    {
        playerDistance = player.transform.position.x - transform.position.x;
        
        float direction = Mathf.Sign(playerDistance);
        if (direction == 1 && !facingRight)
        {
            transform.Rotate(Vector2.up * 180);
            facingRight = true;
        }
        else if (direction == -1 && facingRight)
        {

            transform.Rotate(Vector2.up * 180);
            facingRight = false;
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    

    public void Shoot()
    {
        gun.Shoot();
    }
}
