using Combat;
using Levels;
using UnityEngine;

namespace Enemies
{
    public class BlueEnemy : Enemy
    {
        /*[Header(nameof(BlueEnemy))]*/
        
        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();

            var trajectory = playerPosition - (Vector2)transform.position;
            enemyRigidbody.linearVelocity = trajectory * (Time.fixedDeltaTime * moveSpeed);
        }
    }
}