using System;
using UnityEngine;
using UnityEngine.AI;

using Levels;

namespace Characters
{
    public abstract class Enemy : Character
    {
        public event Action<Enemy> OnDeath;

        [Header(nameof(Enemy))]
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] private LevelColor levelColor;
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

        public Collider2D EnemyCollider => characterCollider;
        public LevelColor LevelColor => levelColor;
        public SpriteRenderer SpriteRenderer => GetComponentInChildren<SpriteRenderer>();
        public float AttackDamage => attackDamage;

        public virtual void Initialize(Player player, Transform weaponProjectileContainer)
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
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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
                characterRigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            ToggleProjectiles(true);
            playerPosition = player.transform.position;

            if (!IsNearPlayer())
            {
                MoveTowardsPlayer();
                return;
            }

            characterRigidbody.linearVelocity = Vector2.zero;
            TryAttackPlayer();
        }

        protected virtual void ToggleProjectiles(bool toggle)
        {

        }

        protected virtual void MoveTowardsPlayer()
        {
            // navMeshAgent.SetDestination(playerPosition);

            // Flip sprite depending on player position
            characterSpriteRenderer.flipX = playerPosition.x < transform.position.x;
        }

        protected bool IsNearPlayer()
        {
            var distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
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
            ToggleProjectiles(currentState is EnemyState.Attacking);
            gameObject.SetActive(currentState is EnemyState.Attacking);
        }

        public void Death()
        {
            // Update enemy kill count logic
            Level.Instance.UpdateEnemyKilledCount(Level.Instance.currentColor);

            // Update enemy object
            characterRigidbody.freezeRotation = false;
            characterCollider.enabled = false;
            currentState = EnemyState.Dead;
            
            // Destroy the enemy object after a delay
            Destroy(gameObject, 2f);
            OnDeath?.Invoke(this);
        }
    }
}