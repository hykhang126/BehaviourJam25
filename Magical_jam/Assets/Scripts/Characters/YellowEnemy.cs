using System;
using UnityEngine;

namespace Characters
{
    public class YellowEnemy : Enemy
    {
        [Header(nameof(YellowEnemy))]
        [SerializeField] private Collider2D enemyTriggerCollider;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private bool isDashing;
        private float speedPerFrame;
        private float speedTimer;

        public override void Initialize(Player player, Transform weaponProjectileContainer)
        {
            base.Initialize(player, weaponProjectileContainer);
            
            Physics2D.IgnoreCollision(EnemyCollider, player.PlayerCollider);
            speedPerFrame = Time.deltaTime * moveSpeed;
        }

        private void FixedUpdate()
        {
            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
                return;
            }

            if (health <= 0)
            {
                Death();
                return;
            }

            if (currentState is EnemyState.Dormant or EnemyState.Dead)
            {
                isDashing = false;
                characterRigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            if (isDashing && (Vector2)transform.position != endPosition)
            {
                speedTimer += speedPerFrame;
                transform.position = Vector2.Lerp(startPosition, endPosition, speedTimer);
                lastAttackTime = Time.time;
                return;
            }

            isDashing = false;
            TryAttackPlayer();
        }

        protected override void TryAttackPlayer()
        {
            if (Time.time - lastAttackTime < attackCooldown || currentState is not EnemyState.Attacking || isDashing)
            {
                return;
            }

            startPosition = transform.position;
            playerPosition = player.transform.position;
            endPosition = playerPosition - (Vector2)transform.position;
            isDashing = true;
            speedTimer = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Collider2D>() == player.PlayerCollider)
            {
                player.TakeDamage(attackDamage);
            }
        }
    }
}