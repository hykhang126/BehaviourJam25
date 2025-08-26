using System;
using Characters;
using UnityEngine;

namespace Utility
{
    public class PunchFist : Weapon
    {
        public event Action OnFinished;
        
        [SerializeField] private Enemy enemyOwner;
        [SerializeField] private Rigidbody2D punchFistRigidbody;
        [SerializeField] private Collider2D punchFistCollider;
        [SerializeField] private Collider2D punchFistTriggerCollider;
        [SerializeField] private AnimationCurve movementAnimationCurve;
        [SerializeField] private float duration;
        [SerializeField] private float distance;
        
        private Player player;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private float punchStartTime;
        private bool isPunching;
        
        public bool IsPunching => isPunching;

        public void Initialize(Player player, Vector2 normalizedTrajectoryToPlayer)
        {
            this.player = player;
            isPunching = true;
            punchStartTime = Time.time;
            startPosition = transform.position;
            endPosition = startPosition + normalizedTrajectoryToPlayer * distance;
            
            Physics2D.IgnoreCollision(punchFistCollider, player.PlayerCollider);
        }

        private void Update()
        {
            if (!isPunching)
            {
                return;
            }
            
            if (Time.time - punchStartTime > duration)
            {
                FinishPunch();
                return;
            }

            var currentCurveRatio = (Time.time - punchStartTime) / duration;
            var currentCurvePosition = GetCurvePositionFromRatio(currentCurveRatio);
            var currentTransformPosition = Vector2.Lerp(startPosition, endPosition, currentCurvePosition);
            transform.position = currentTransformPosition;
        }

        private float GetCurvePositionFromRatio(float ratio)
        {
            return movementAnimationCurve.Evaluate(ratio);
        }

        private void FinishPunch()
        {
            isPunching = false;
            transform.position = startPosition;
            OnFinished?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isPunching)
            {
                return;
            }
            
            if (other == player.PlayerCollider)
            {
                player.TakeDamage(enemyOwner.AttackDamage);
            }
        }
    }
}