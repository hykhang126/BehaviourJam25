using Combat;
using Levels;
using UnityEngine;
using Utility;

namespace Enemies
{
    public class GreenEnemy : Enemy
    {
        [Header(nameof(GreenEnemy))]
        [SerializeField] private PunchFist punchFist;
        
        protected override void MoveTowardsPlayer()
        {
            if (punchFist.IsPunching)
            {
                enemyRigidbody.linearVelocity = Vector2.zero;
                return;
            }
            
            base.MoveTowardsPlayer();
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