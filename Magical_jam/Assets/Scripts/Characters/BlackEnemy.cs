using UnityEngine;
using Random = System.Random;

namespace Characters
{
    public class BlackEnemy : Enemy
    {
        [Header(nameof(BlackEnemy))]
        [SerializeField] private float lerpSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float randomMovementTrajectoryDuration;    // Total duration of each instances of random movement
        [SerializeField] private float randomMovementTrajectoryCooldown;    // This will be less than the duration
        [SerializeField] private float stunDuration;
        
        private Random random;
        private Vector2 currentMovementTrajectory;
        
        // Random movement
        private float randomMovementCooldownTimer;
        private float randomMovementLerpVariable;
        private Vector2 randomMovementTrajectoryTarget;
        
        // Attack
        private bool foundPlayer;
        private bool isAttacking;
        private float attackCooldownTimer;
        
        // Stun
        private bool isStunned;
        private float stunDurationTimer;
        
        private const int kRandomExtrema = 100;

        private void Awake()
        {
            random = new Random();
            ResetMovementTrajectory();
        }

        private void FixedUpdate()
        {
            // If enemy is stunned, it cannot do anything until its timer counts down.
            if (isStunned)
            {
                if (Time.time - stunDurationTimer > stunDuration)
                {
                    attackCooldownTimer = Time.time;
                    isStunned = false;
                }
                
                return;
            }
            
            // If player is found, run into range and attack.
            if (foundPlayer && !isAttacking && Time.time - attackCooldownTimer > attackCooldown)
            {
                isAttacking = true;
            }
            
            if (isAttacking)
            {
                var playerTransformPosition = (Vector2)player.transform.position;
                var enemyTransformPosition = (Vector2)transform.position;
                
                if (Vector2.Distance(playerTransformPosition, enemyTransformPosition) > attackRange)
                {
                    characterRigidbody.linearVelocity = (playerTransformPosition - enemyTransformPosition).normalized * (Time.fixedDeltaTime * runSpeed);
                    return;
                }
                
                player.TakeDamage(attackDamage);
                foundPlayer = false;
                isAttacking = false;
                attackCooldownTimer = Time.time;
                return;
            }
            
            // Else, move randomly.
            var timeSinceCooldown = Time.time - randomMovementCooldownTimer;
            
            if (timeSinceCooldown < randomMovementTrajectoryCooldown)
            {
                FindNewMovementTrajectory();
            }
            else if (timeSinceCooldown < randomMovementTrajectoryDuration)
            {
                LerpMovementTrajectory();
            }
            else
            {
                randomMovementCooldownTimer = Time.time;
                randomMovementLerpVariable = 0;
            }
            
            characterRigidbody.linearVelocity = currentMovementTrajectory * (Time.fixedDeltaTime * moveSpeed);
            
            if (IsPlayerWithinVision())
            {
                foundPlayer = true;
            }
        }

        private bool IsPlayerWithinVision()
        {
            var raycastHits = Physics2D.RaycastAll(transform.position, currentMovementTrajectory.normalized, 500);
            foreach (var raycastHit in raycastHits)
            {
                if (raycastHit.collider == player.PlayerCollider)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void ResetMovementTrajectory()
        {
            currentMovementTrajectory = new Vector2(random.Next(-1 * kRandomExtrema, kRandomExtrema), random.Next(-1 * kRandomExtrema, kRandomExtrema));
            randomMovementCooldownTimer = Time.time;
        }
        
        private void FindNewMovementTrajectory()
        {
            randomMovementTrajectoryTarget = new Vector2(random.Next(-1 * kRandomExtrema, kRandomExtrema), random.Next(-1 * kRandomExtrema, kRandomExtrema));
        }
        
        private void LerpMovementTrajectory()
        {
            randomMovementLerpVariable += lerpSpeed * Time.fixedDeltaTime;
            currentMovementTrajectory = Vector2.Lerp(currentMovementTrajectory, randomMovementTrajectoryTarget, randomMovementLerpVariable);
        }

        public void GetStunned()
        {
            characterRigidbody.linearVelocity = Vector2.zero;
            isStunned = true;
            isAttacking = false;
            stunDurationTimer = Time.time;
            Debug.LogError("black enemy STUNNED");
        }
    }
}