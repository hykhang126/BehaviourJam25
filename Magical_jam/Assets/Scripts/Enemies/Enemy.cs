using Combat;
using Levels;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header(nameof(Enemy))]
        [SerializeField] protected Rigidbody2D enemyRigidbody;
        [SerializeField] private Collider2D enemyCollider;
        [SerializeField] private LevelColor levelColor;
        [SerializeField] private float health;
        [SerializeField] protected float moveSpeed;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCooldown;
        
        private EnemyState currentState;
        private Player player;
        private float lastAttackTime;
        protected Vector2 playerPosition;
        protected Transform weaponProjectileContainer;

        public Collider2D EnemyCollider => enemyCollider;
        public LevelColor LevelColor => levelColor;
        
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

        private void Update()
        {
            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
            }
            
            if (currentState is EnemyState.Dormant or EnemyState.Dead)
            {
                return;
            }

            if (health <= 0)
            {
                Death();
            }
            
            MoveTowardsPlayer();
            TryAttackPlayer();
        }

        protected virtual void MoveTowardsPlayer()
        {
            playerPosition = player.transform.position;
        }

        private bool IsNearPlayer()
        {
            var distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
            return distanceToPlayer <= attackRange;
        }

        private void TryAttackPlayer()
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
        }
        
        public void Death()
        {
            enemyRigidbody.freezeRotation = false;
            currentState = EnemyState.Dead;
            Destroy(this);
        }
    }
}