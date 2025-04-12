using UnityEngine;
using UnityEngine.Serialization;

using Combat;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HUD HUD;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerArm;
    [SerializeField] private Gun attachedGun;
    [SerializeField] private int health;
    [SerializeField] float moveSpeed;

    [FormerlySerializedAs("_characterBody")] [SerializeField] private Transform _playerBody;

    [SerializeField] bool isHit;

    Animator animator;

    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    [SerializeField] private LevelColor _currentColor;

    //Awake is called before the game even starts.
    void Awake()
    {
        isHit = false;
    }

    // Get health
    public int getHealth()
    {
        return health;
    }

    // TakeDamage
    // This function is called when the player takes damage
    // It decreases the player's health by 1 and updates the HUD
    public void TakeDamage()
    {
        health--;
        HUD.lowerHealth();
        isHit = true;
        if (health == 0)
        {
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
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Initialize the animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }

        // Initialize the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }
    
    void Update()
    {
        var playerScreenPointPosition = playerCamera.WorldToScreenPoint(transform.position);
        
        // Update player rotation
        var flipPlayer = Input.mousePosition.x < playerScreenPointPosition.x;
        var playerBodyRotation = playerBody.rotation;
        playerBodyRotation.y = flipPlayer ? 180f : 0f;
        playerBody.rotation = playerBodyRotation;
        
        // Update arm rotation
        var armVector = Input.mousePosition - playerScreenPointPosition;
        var armVectorAbs = new Vector3(Mathf.Abs(armVector.x), Mathf.Abs(armVector.y), Mathf.Abs(armVector.z));
        var armAngle = Vector3.Angle(armVectorAbs, Vector3.right);
        var armRotationZ = (armVector.y >= 0 ? 1 : -1) * armAngle;
        playerArm.rotation = Quaternion.Euler(0f, playerBodyRotation.y, armRotationZ);
    }

    public void TryShoot(Vector3 mouseScreenPointPosition)
    {
        if (!attachedGun)
        {
            Debug.LogError($"No gun attached to player {gameObject.name}.");
            return;
        }
        
        attachedGun.Shoot(playerCamera.ScreenToViewportPoint(mouseScreenPointPosition));
    }
}

