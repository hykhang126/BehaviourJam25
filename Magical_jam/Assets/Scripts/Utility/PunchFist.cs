using System;
using Enemies;
using UnityEngine;

namespace Utility
{
    public class PunchFist : MonoBehaviour
    {
        public event Action OnFinished;
        
        [SerializeField] private Enemy enemyOwner;
        [SerializeField] private Rigidbody2D punchFistRigidbody;
        [SerializeField] private Collider2D punchFistCollider;
        [SerializeField] private AnimationCurve movementAnimationCurve;
        [SerializeField] private float duration;
        [SerializeField] private float distance;
        
        private Player player;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private float punchStartTime;
        private bool isPunching;
        
        public bool IsPunching => isPunching;

        private void Awake()
        {
            startPosition = transform.localPosition;
        }

        public void Initialize(Player player, Vector2 normalizedTrajectoryToPlayer)
        {
            this.player = player;
            endPosition = startPosition + normalizedTrajectoryToPlayer * distance;
            isPunching = true;
            punchStartTime = Time.time;
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
            transform.localPosition = currentTransformPosition;
        }

        private float GetCurvePositionFromRatio(float ratio)
        {
            return movementAnimationCurve.Evaluate(ratio);
        }

        private void FinishPunch()
        {
            isPunching = false;
            transform.localPosition = startPosition;
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