using UnityEngine;
using Enemies;

public class BarkingKitten : MonoBehaviour
{
    Animator animator;

    [SerializeField] float barkRadius;

    AudioSource barkSound;

    [SerializeField] float jumpSpeed;

    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;
    [SerializeField] float speed;
    float direction;
    bool idle;

    bool walking;

    public Player player;

    float distance;

    public float totalDistance;
    // Start is called before the first frame update
    void Start()
    {
        idle = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        barkSound = GetComponent<AudioSource>();
    }

    public void Jump(){
        if(rb.linearVelocity.y==0){
            /*if(isCrouching){
                this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                this.transform.position += Vector3.up * crouch;
                speed = speed/crouch;
                isCrouching = false;
            }*/
            rb.AddForce(new Vector2(0f,jumpSpeed*10));
        }
    }

    public void Bark(){
       barkSound.Play();
       Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,barkRadius);
        foreach(Collider2D c in colliders){
            if(c.gameObject.CompareTag("Enemy")){
                c.GetComponent<Enemy>().Death();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(idle){
            distance = player.transform.position.x - transform.position.x ;
            direction = Mathf.Sign(distance);
            distance = Mathf.Abs(distance);
            if(distance>=totalDistance){
                idle = false;
                walking = true;
                animator.SetBool("isMoving",true);
            }
        }
        if(walking){
            transform.Translate(Vector3.right*direction*Time.deltaTime*speed);
            distance = transform.position.x - player.transform.position.x;
            if(direction>0){
                spriteRenderer.flipX = false;
            }
            else{
                spriteRenderer.flipX = true;
            }
            distance = Mathf.Abs(distance);
            if(distance<1){
                idle = true;
                walking = false;
                animator.SetBool("isMoving",false);
            }
        }
        if(rb.linearVelocity.y!=0){
            animator.SetBool("isJumping",true);
        }
        else{
            animator.SetBool("isJumping",false);
        }
    }
}
