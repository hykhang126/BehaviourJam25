using UnityEngine;
using UnityEngine.Serialization;

using Combat;
using System;
using Levels;
using Utility;
using System.Threading;
using Core;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HUD HUD = HUD.instance;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerArm;
    [SerializeField] private Kite attachedKite;
    [SerializeField] private float health;
    [SerializeField] private float MAX_HEALTH = 100f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float blinkCooldown = 0.1f;
    [SerializeField] private Core.Timer hitTimer;

    [Header("Player Sprite Logic")]
    [SerializeField] private Shield shield;
    [SerializeField] private Gun attachedGun;
    [SerializeField] private SpriteRenderer playerBunSpriteRenderer;

    [Header("Player Color")]
    [SerializeField] private LevelColor _currentColor;
    // Update the player's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdatePlayerColor(LevelColor newColor)
    {
        _currentColor = newColor;
    }
    
    public bool isHit;

    public float hitDuration = 1f;

    public bool isInvincible;

    public Collider2D PlayerCollider => playerCollider;
    
    PlayerController playerController;

    Melee melee;

    Rigidbody2D rb;

    Animator animator;

    PlayerWeaponAnimCon playerWeaponAnimController;

    SpriteRenderer spriteRenderer;

    float blinkDuration;

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

        if (playerBunSpriteRenderer == null)
        {
            Debug.LogError("Player bun sprite renderer not found in the children of the player object.");
        }

        health = MAX_HEALTH;
        isHit = isInvincible = false;

        blinkDuration = (hitDuration > 0f) ? hitDuration : 1f; // Initialize blink timer
    }
    
    void Update()
    {
        var playerScreenPointPosition = playerCamera.WorldToScreenPoint(transform.position);

        // Timer tick
        if (hitTimer != null && hitTimer.IsRunning())
        {
            hitTimer.Tick(Time.deltaTime);
        }

        // If player is hit, blinking
        if (isHit)
        {
            if (blinkDuration > 0f)
            {
                blinkDuration -= Time.deltaTime;
            }
            else
            {
                spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle the sprite renderer
                blinkDuration = blinkCooldown; // Reset the timer
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
        
        // Update weapons
        playerWeaponAnimController.ToggleGun(_currentColor is LevelColor.Red);
        shield.ToggleShield(_currentColor is LevelColor.Blue);
        melee.ToggleMelee(_currentColor is LevelColor.Green);
        attachedKite.ToggleKite(_currentColor is LevelColor.Yellow);

        // Check if moving to trigger gun and shield bash animation
        playerWeaponAnimController.SetFloatAnimation("Speed", moveSpeed);
        shield.SetShieldFloat("Speed", moveSpeed);

        // Update HUD logic
        // Whenever player is hit, trigger the heart animation
        // HUD.UpdateHealthBar(health / MAX_HEALTH);
        
    }

    // Get health
    public float GetHealth()
    {
        return health;
    }

    // Get max health
    public float GetMaxHealth()
    {
        return MAX_HEALTH;
    }
    
    // function to check if player can be damaged
    public bool CanBeDamaged()
    {
        return !isHit && !isInvincible;
    }

    // TakeDamage
    // This function is called when the player takes damage
    // It decreases the player's health by 1 and updates the HUD
    public void TakeDamage(float damageTaken = 1)
    {
        if (isHit || isInvincible)
        {
            // If the player is already hit or invincible, do not take damage
            return;
        }

        health -= damageTaken;
        // Animator
        animator.SetTrigger("isHit");
        HUD.SetHeartAnimationTrigger("isHit");
        /*HUD.lowerHealth();*/
        isHit = true;
        hitTimer = new Core.Timer(hitDuration);
        hitTimer.onTimerEnd += () => isHit = false; // Reset isHit after the hit duration
        if (health <= 0)
        {
            HUD.SetHeartAnimationBool("isDead", true);
            HUD.GameOver();
            Destroy(this.gameObject);
        }
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

    // Set if the player is invincible
    public void SetIsInvincible(bool invincible){
        isInvincible = invincible;
    }

    // Set player's bun visibility
    public void SetBunVisibility(bool visible)
    {
        if (playerBunSpriteRenderer != null)
        {
            playerBunSpriteRenderer.enabled = visible;
        }
        else
        {
            Debug.LogError("Player bun sprite renderer not found in the children of the player object.");
        }
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
            // Green is melee
            case LevelColor.Green:
                // Perform action for green color
                melee.gameObject.SetActive(true);
                melee.Attack();
                break;
            // Yellow is kite
            case LevelColor.Yellow:
                // Perform action for yellow color
                TryMoveKite(Input.mousePosition);
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

    private void TryShoot(Vector3 mouseScreenPointPosition)
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

    private void TryMoveKite(Vector3 mouseScreenPointPosition)
    {
        if (!attachedKite)
        {
            Debug.LogError($"No gun attached to player {gameObject.name}.");
            return;
        }
        
        playerWeaponAnimController.SetTriggerAnimation("Shoot");
        attachedKite.Move(playerCamera.ScreenToWorldPoint(mouseScreenPointPosition));
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

