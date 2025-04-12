using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class Melee : MonoBehaviour
{
    Player player;

    Animator animator;

    [SerializeField] GameObject meleeCapsuleCenter;

    bool isAttacking;
    
    void Awake(){
        player = GetComponentInParent<Player>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }

        isAttacking = false;
    }

    public bool getisAttacking(){
        return isAttacking;
    }

    public void shouldFlipMeleeCapsuleCenter(){
        // Check mouse position to see if we should flip the sprite
    }

    public void Attack(){
        if(isAttacking) return; // Prevents multiple attacks at once
        animator.SetTrigger("Action"); // Trigger the attack animation
        isAttacking = true;
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(meleeCapsuleCenter.transform.position,new Vector2(3,2),CapsuleDirection2D.Vertical,0);
        foreach(Collider2D c in colliders){
            if(c.CompareTag("Enemy")){
                c.GetComponent<Enemy>().Death();
            }
            else if(c.CompareTag("Destructible")){
                Debug.Log("BOOM,CRASH,POW!! DESTRUCTIBLE DESTROYED");
            }
        }
    }

    public void EndAttack(){
        isAttacking = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
