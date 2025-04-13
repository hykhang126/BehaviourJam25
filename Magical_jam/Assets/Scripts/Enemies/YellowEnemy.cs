using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class YellowEnemy : Enemy
    {
        private Coroutine dashCoroutine;
        private Vector2 endPosition;
        private bool isDashing;
        private float distanceCovered;
        
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
                StopDashing();
                enemyRigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }
            
            TryAttackPlayer();
            
            gameObject.SetActive(true);
        }

        protected override void TryAttackPlayer()
        {
            if (Time.time - lastAttackTime < attackCooldown || currentState is not EnemyState.Attacking || isDashing)
            {
                return;
            }
            
            StartDashing();
        }

        private void StartDashing()
        {
            playerPosition = player.transform.position;
            endPosition = playerPosition - (Vector2)transform.position;
            // End position detected here. Visual indicator required.
            
            StopDashing();
            dashCoroutine = StartCoroutine(Dash());
            
            lastAttackTime = Time.time;
            isDashing = true;
        }

        private void StopDashing()
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            
            isDashing = false;
            distanceCovered = 0f;
        }

        private IEnumerator Dash()
        {
            while (distanceCovered < attackRange)
            {
                enemyRigidbody.linearVelocity = endPosition * moveSpeed;
                distanceCovered += enemyRigidbody.linearVelocity.magnitude;
                Debug.LogError(distanceCovered);
                yield return null;
            }

            enemyRigidbody.linearVelocity = Vector2.zero;
        }
    }
}