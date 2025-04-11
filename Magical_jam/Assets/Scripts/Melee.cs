using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    Player player;

    [SerializeField] GameObject meleeCapsuleCenter;

    bool isAttacking;
    
    void Awake(){
        player = GetComponentInParent<Player>();
        isAttacking = false;
    }

    public bool getisAttacking(){
        return isAttacking;
    }

    public void flipMeleeCapsuleCenter(){
        meleeCapsuleCenter.transform.localPosition = new Vector3(meleeCapsuleCenter.transform.localPosition.x-14, meleeCapsuleCenter.transform.localPosition.y, meleeCapsuleCenter.transform.localPosition.z);
    }

    public void unFlipMeleeCapsuleCenter(){
        meleeCapsuleCenter.transform.localPosition = new Vector3(meleeCapsuleCenter.transform.localPosition.x+14, meleeCapsuleCenter.transform.localPosition.y, meleeCapsuleCenter.transform.localPosition.z);
    }

    public void Attack(){
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
