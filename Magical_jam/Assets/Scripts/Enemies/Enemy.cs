using System;
using Combat;
using Levels;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public event Action<Enemy> OnDeath;
        
        [Header(nameof(Enemy))]
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Rigidbody2D enemyRigidbody;
        [SerializeField] private Collider2D enemyCollider;
        [SerializeField] private LevelColor levelColor;
        [SerializeField] protected float health;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float attackCooldown;
        [Header("Knockback when hit should be small 0.5f - 1.5f")]
        [SerializeField] private float knockbackForce;

        protected EnemyState currentState;
        protected Player player;
        protected float lastAttackTime;
        protected Vector2 playerPosition;
        protected Vector2 normalizedTrajectoryToPlayer;
        protected Transform weaponProjectileContainer;

        public Collider2D EnemyCollider => enemyCollider;
        public LevelColor LevelColor => levelColor;
        public float AttackDamage => attackDamage;
        
        public void Initialize(Player player, Transform weaponProjectileContainer)
        {
            this.player = player;
            this.weaponProjectileContainer = weaponProjectileContainer;

            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
            }
            
            lastAttackTime = Time.time;
            currentState = EnemyState.Attacking;
        }
        
        private void Start()
        {
            if (!player) 
            {
                player = FindFirstObjectByType<Player>();
                if (!player)
                {
                    Debug.LogError("Player not found in the scene.");
                    Destroy(this);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
            }

            if (health <= 0)
            {
                Death();
            }
            
            if (currentState is EnemyState.Dormant or EnemyState.Dead)
            {
                enemyRigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }
            
            MoveTowardsPlayer();
            TryAttackPlayer();
            
            ToggleProjectiles(true);
            gameObject.SetActive(true);
        }

        protected virtual void ToggleProjectiles(bool toggle)
        {
            
        }

        protected virtual void MoveTowardsPlayer()
        {
            playerPosition = player.transform.position;
            normalizedTrajectoryToPlayer = (playerPosition - (Vector2)transform.position).normalized;
            enemyRigidbody.linearVelocity = normalizedTrajectoryToPlayer * (Time.fixedDeltaTime * moveSpeed);

            // Flip sprite dependiong on player position
            if (playerPosition.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        protected bool IsNearPlayer()
        {
            var distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
            return distanceToPlayer <= attackRange;
        }

        protected virtual void TryAttackPlayer()
        {
            if (!IsNearPlayer() || Time.time - lastAttackTime < attackCooldown || currentState is not EnemyState.Attacking)
            {
                return;
            }

            player.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
        
        public void TakeDamage(float damageTaken)
        {
            health -= damageTaken;

            // Knockback effect
            var knockbackDirection = (Vector2)transform.position - playerPosition;
            knockbackDirection.Normalize();
            transform.position += (Vector3)knockbackDirection * knockbackForce;
        }

        public void SetEnemyState(EnemyState enemyState)
        {
            currentState = enemyState;
            ToggleProjectiles(false);
            gameObject.SetActive(enemyState is EnemyState.Attacking);
        }
        
        public void Death()
        {
            enemyRigidbody.freezeRotation = false;
            currentState = EnemyState.Dead;
            // Destroy the enemy object after a delay
            Destroy(gameObject, 2f);
            OnDeath?.Invoke(this);
        }
    }
}