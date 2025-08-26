using Levels;
using UnityEngine;
using Utility;

namespace Characters
{
    public class Player : Character
    {
        [Header(nameof(Player))]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private HUD HUD = HUD.instance;
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform playerArm;
        [SerializeField] private Kite attachedKite;
        [SerializeField] private float MAX_HEALTH = 100f;
        [SerializeField] private float blinkCooldown = 0.1f;
        [SerializeField] private Core.Timer hitTimer;

        [Header("Player Sprite Logic")]
        [SerializeField] private Shield shield;
        [SerializeField] private Gun attachedGun;
        [SerializeField] private SpriteRenderer playerArmSpriteRenderer;

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

        public Collider2D PlayerCollider => characterCollider;

        private Melee melee;

        private Rigidbody2D rb;

        private Animator animator;

        private SpriteRenderer spriteRenderer;

        private float blinkDuration;

        // Start is called before the first frame update
        void Start()
        {
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

            if (playerArmSpriteRenderer == null)
            {
                Debug.LogError("Player arm sprite renderer not found in the children of the player object.");
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
            attachedGun.ToggleGun(_currentColor is LevelColor.Red);
            shield.ToggleShield(_currentColor is LevelColor.Blue);
            melee.ToggleMelee(_currentColor is LevelColor.Green);

            // Check if moving to trigger gun and shield bash animation
            attachedGun.SetFloatAnimation("Speed", moveSpeed);
            shield.SetFloatAnimation("Speed", moveSpeed);

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

        // Set player's arm visibility
        public void SetArmVisibility(bool visible)
        {
            if (playerArmSpriteRenderer != null)
            {
                playerArmSpriteRenderer.enabled = visible;
            }
            else
            {
                Debug.LogError("Player arm sprite renderer not found in the children of the player object.");
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
                    TryMelee();
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
        
        private void TryMelee()
        {
            if (!melee)
            {
                Debug.LogError($"No melee attached to player {gameObject.name}.");
                return;
            }

            melee.ShouldFlipMeleeCapsuleCenter();
            melee.gameObject.SetActive(true); // Enable the melee object
            melee.Attack();
        }

        private void TryMoveKite(Vector3 mouseScreenPointPosition)
        {
            if (!attachedKite)
            {
                Debug.LogError($"No gun attached to player {gameObject.name}.");
                return;
            }

            attachedGun.SetTriggerAnimation("Shoot");
            attachedKite.Move(playerCamera.ScreenToWorldPoint(mouseScreenPointPosition));
        }

        public void OnCollisionEnter2D(Collision2D collision)
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
                shield.SetBoolAnimation(value, state); // Set the trigger for the shield animation
            }
        }
    }
}

