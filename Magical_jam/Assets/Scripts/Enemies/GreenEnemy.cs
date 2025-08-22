using Combat;
using Levels;
using UnityEngine;
using Utility;

namespace Characters
{
    public class GreenEnemy : Enemy
    {
        [Header(nameof(GreenEnemy))]
        [SerializeField] private Transform enemyBody;
        [SerializeField] private PunchFist punchFist;
        
        protected override void MoveTowardsPlayer()
        {
            if (punchFist.IsPunching)
            {
                characterRigidbody.linearVelocity = Vector2.zero;
                return;
            }
            
            playerPosition = player.transform.position;
            normalizedTrajectoryToPlayer = (playerPosition - (Vector2)transform.position).normalized;
            characterRigidbody.linearVelocity = normalizedTrajectoryToPlayer * (Time.fixedDeltaTime * moveSpeed);
            
            var enemyBodyRotation = enemyBody.rotation;
            enemyBodyRotation.y = playerPosition.x < transform.position.x ? 180f : 0f;
            enemyBody.rotation = enemyBodyRotation;
        }

        protected override void TryAttackPlayer()
        {
            if (!IsNearPlayer() || Time.time - lastAttackTime < attackCooldown || currentState is not EnemyState.Attacking || punchFist.IsPunching)
            {
                return;
            }
            
            PunchFist();
        }

        private void PunchFist()
        {
            punchFist.Initialize(player, normalizedTrajectoryToPlayer);
            punchFist.OnFinished += HandlePunchFinished;
        }
        
        private void HandlePunchFinished()
        {
            lastAttackTime = Time.time;
            punchFist.OnFinished -= HandlePunchFinished;
        }
    }
}