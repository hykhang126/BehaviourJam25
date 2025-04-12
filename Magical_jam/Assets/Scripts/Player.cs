using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    [SerializeField] float jumpForce;

    [SerializeField] HUD HUD;

    Animator animator;

    SpriteRenderer spriteRenderer;

    bool isHit;

    [SerializeField] int health;

    //Awake is called before the game even starts.
    void Awake(){

        spriteRenderer = GetComponent<SpriteRenderer>();

        isHit = false;
    }

    // Get health
    public int getHealth(){
        return health;
    }

    // TakeDamage
    // This function is called when the player takes damage
    // It decreases the player's health by 1 and updates the HUD
    public void TakeDamage(){
        health--;
        HUD.lowerHealth();
        isHit = true;
        if(health==0){
            HUD.GameOver();
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerCamera == null){
            playerCamera = Camera.main;
        }

        // Initialize the animator component
        animator = GetComponent<Animator>();
        if(animator == null){
            animator = gameObject.AddComponent<Animator>();
        }

        // Initialize the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        if(rb == null){
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.flipX = Input.mousePosition.x < playerCamera.WorldToScreenPoint(transform.position).x;
    }
}
