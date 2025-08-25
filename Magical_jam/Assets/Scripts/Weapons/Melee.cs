using UnityEngine;
using Characters;

public class Melee : Weapon
{
    Player player;

    [SerializeField] GameObject meleeCapsuleCenter;

    [SerializeField] float damage = 30f;
    
    private bool projectileToggle;
    bool isAttacking;
    
    void Awake()
    {
        player = GetComponentInParent<Player>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }

        isAttacking = false;
    }

    public bool GetisAttacking()
    {
        return isAttacking;
    }

    public void ShouldFlipMeleeCapsuleCenter()
    {
        // Check mouse position to see if we should flip the sprite
    }

    public void Attack()
    {
        if(isAttacking) return; // Prevents multiple attacks at once
        animator.SetTrigger("Melee"); // Trigger the attack animation
        isAttacking = true;
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(meleeCapsuleCenter.transform.position,new Vector2(3,2),CapsuleDirection2D.Vertical,0);
        foreach(Collider2D c in colliders){
            if(c.gameObject.CompareTag("Enemy")){
                c.GetComponentInParent<Enemy>().TakeDamage(damage);
            }
            else if(c.gameObject.CompareTag("Destructible")){
                Debug.Log("BOOM,CRASH,POW!! DESTRUCTIBLE DESTROYED");
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;

        // Disable this object
        gameObject.SetActive(false);
    }
    
    public void ToggleMelee(bool toggle)
    {
        if (projectileToggle == toggle)
        {
            return;
        }
        
        projectileToggle = toggle;
        
        if (!projectileToggle)
        {
            // End attack animation as soon as we toggle off
            EndAttack();
        }
    }
}
