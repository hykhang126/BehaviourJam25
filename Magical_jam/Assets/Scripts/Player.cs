using UnityEngine;
using UnityEngine.Serialization;

using Combat;
using System;
using Levels;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HUD HUD = HUD.instance;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerArm;
    [SerializeField] private Gun attachedGun;
    [SerializeField] private float health;
    [SerializeField] private float MAX_HEALTH = 100f;
    [SerializeField] float moveSpeed;

    public bool isHit;

    PlayerController playerController;

    Melee melee;

    Rigidbody2D rb;

    Animator animator;

    PlayerWeaponAnimCon playerWeaponAnimController;

    SpriteRenderer spriteRenderer;

    [SerializeField] private float blinkTimer;
    [SerializeField] private float blinkCooldown = 0.1f;

    [SerializeField] Shield shield;

    [SerializeField] private LevelColor _currentColor;

    
    public bool IsHit => isHit;
    public Collider2D PlayerCollider => playerCollider;

    //Awake is called before the game even starts.
    void Awake()
    {
        isHit = false;
    }

    // Get health
    public float getHealth()
    {
        return health;
    }

    // TakeDamage
    // This function is called when the player takes damage
    // It decreases the player's health by 1 and updates the HUD
    public void TakeDamage(float damageTaken = 1)
    {
        if (isHit)
        {
            return;
        }

        health -= damageTaken;
        // Animator
        animator.SetTrigger("isHit");
        /*HUD.lowerHealth();*/
        isHit = true;
        if (health <= 0)
        {
            HUD.GameOver();
            Destroy(this.gameObject);
        }
    }
    
    // Update the player's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdatePlayerColor(LevelColor newColor)
    {
        _currentColor = newColor;
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
        // Initialize the player controller
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = gameObject.AddComponent<PlayerController>();
        }

        melee = GetComponentInChildren<Melee>();
        if (melee == null)
        {
            Debug.LogError("Melee component not found in the children of the player object.");
        }
        // Hide the melee obj
        melee.gameObject.SetActive(false);

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Initialize the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        if (shield == null)
        {
            shield = GetComponentInChildren<Shield>();
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found in the children of the player object.");
        }

        if (playerWeaponAnimController == null)
        {
            playerWeaponAnimController = GetComponentInChildren<PlayerWeaponAnimCon>();
        }

        if (HUD == null)
        {
            HUD = FindFirstObjectByType<HUD>();
        }
        if (HUD == null)
        {
            Debug.LogError("HUD component not found in the scene.");
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found in the children of the player object.");
        }

        health = MAX_HEALTH;
        isHit = false;
    }
    
    void Update()
    {
        var playerScreenPointPosition = playerCamera.WorldToScreenPoint(transform.position);

        // If player is hit, blinking
        if (isHit)
        {
            if (blinkTimer > 0f)
            {
                blinkTimer -= Time.deltaTime;
            }
            else
            {
                spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle the sprite renderer
                blinkTimer = blinkCooldown; // Reset the timer
            }
        }
        else
        {
            spriteRenderer.enabled = true; // Ensure the sprite renderer is enabled
        }
        
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

        // Update shield collider
        if (_currentColor != LevelColor.Blue)
        {
            shield.DisableShield();
            shield.TurnOffShieldSprite();
        }

        // Update gun sprite
        if (_currentColor != LevelColor.Red)
        {
            playerWeaponAnimController.DisableGun();
        }
        else
        {
            playerWeaponAnimController.EnableGun();
        }

        // Check if moving to trigger gun and shield bash animation
        playerWeaponAnimController.SetFloatAnimation("Speed", moveSpeed);
        shield.SetShieldFloat("Speed", moveSpeed);

        // Update HUD logic
        // Grab silder and fill it with current health percentage
        var healthPercentage = health / MAX_HEALTH;
        HUD.UpdateHealthBar(healthPercentage);
        
    }

    /// FIRE ACTION LOGIC
    /// This function is called when the player presses the fire button
    public void FireAction()
    {
        // More logic depending on the color
        switch (_currentColor)
        {
            // Red is shoot
            case LevelColor.Red:
                TryShoot(Input.mousePosition);
                break;
            // Blue is block
            case LevelColor.Blue:
                TryBlock();
                break;
            // Green is 
            case LevelColor.Green:
                            // Perform action for yellow color
                melee.gameObject.SetActive(true);
                melee.Attack();
                

                break;
            // Yellow is melee
            case LevelColor.Yellow:


                break;
            // Black is flashlight
            case LevelColor.Black:
                // Perform action for black color
                break;
            default:
                Debug.Log("No action defined for this color.");
                break;
        }
    }
    
    public void TryShoot(Vector3 mouseScreenPointPosition)
    {
        if (!attachedGun)
        {
            Debug.LogError($"No gun attached to player {gameObject.name}.");
            return;
        }
        
        playerWeaponAnimController.SetTriggerAnimation("Shoot");
        attachedGun.Shoot(playerCamera.ScreenToWorldPoint(mouseScreenPointPosition), GetType().ToString());
    }

    
    private void TryBlock()
    {
        if (!shield)
        {
            Debug.LogError($"No shield attached to player {gameObject.name}.");
            return;
        }

        shield.EnableShield();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
    }

    internal void SetPlayerSpeed(float magnitude)
    {
        moveSpeed = magnitude;
    }

    internal void SetShieldBool(string value, bool state)
    {
        if (shield != null && value != null)
        {
            shield.SetShieldBool(value, state); // Set the trigger for the shield animation
        }
    }
}

