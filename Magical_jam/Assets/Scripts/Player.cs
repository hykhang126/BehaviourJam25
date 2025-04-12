using UnityEngine;
using UnityEngine.Serialization;

using Combat;

public class Player : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    [SerializeField] HUD HUD;

    [FormerlySerializedAs("_characterBody")] [SerializeField] private Transform _playerBody;

    [SerializeField] int health;
    [SerializeField] bool isHit;

    Animator animator;

    SpriteRenderer spriteRenderer;

    [SerializeField] private LevelColor _currentColor;

    //Awake is called before the game even starts.
    void Awake(){

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

    // Update the player's color based on the current level color
    public void UpdatePlayerColor(LevelColor newColor)
    {
        _currentColor = newColor;
        Debug.Log("Player color updated to: " + _currentColor);
    }

    // Get the current color of the player
    public LevelColor GetPlayerColor()
    {
        return _currentColor;
    }

    // Set if the player is hit
    public void SetIsHit(bool hit){
        isHit = hit;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        bool flipPlayer = Input.mousePosition.x < playerCamera.WorldToScreenPoint(transform.position).x;
        var playerRotation = _playerBody.rotation;
        playerRotation.y = flipPlayer ? 180f : 0f;
        _playerBody.rotation = playerRotation;
    }

    // On Destroy
    private void OnDestroy()
    {
        // Perform any cleanup or additional actions here
        // For example, you can log a message or trigger an event
        Debug.Log("Player object destroyed.");
    }
}
