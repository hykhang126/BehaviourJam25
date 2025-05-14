using System;
using Characters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    public class Kite : Weapon
    {
        [SerializeField] private Collider2D kiteCollider;
        [SerializeField] private float kiteSpeed;
        [SerializeField] private float kiteDamage;
        [SerializeField] private LineRenderer laser;
        [SerializeField] private float laserDamage;
        [SerializeField] private Transform handContainer;
        [SerializeField] private Transform projectileContainer;

        private Vector2 initialLocalPosition;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private float kiteSpeedPerFrame;
        private float kiteSpeedTimer;
        private bool isMovingToClick;

        private void Awake()
        {
            initialLocalPosition = transform.localPosition;
            startPosition = transform.position;
            kiteSpeedPerFrame = kiteSpeed * Time.fixedDeltaTime;
            kiteSpeedTimer = 0;
        }

        private void FixedUpdate()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            UpdateLaser();
            
            if (transform.parent == handContainer)
            {
                transform.localPosition = initialLocalPosition;
            }
            
            if (isMovingToClick && (Vector2)transform.position != endPosition)
            {
                kiteSpeedTimer += kiteSpeedPerFrame;
                transform.position = Vector2.Lerp(startPosition, endPosition, kiteSpeedTimer);
                return;
            }
            
            isMovingToClick = false;
        }

        private void UpdateLaser()
        {
            laser.SetPosition(0, (Vector2)transform.position);
            laser.SetPosition(1, (Vector2)handContainer.position);
            
            var raycastHits = Physics2D.RaycastAll(handContainer.position, (Vector2)transform.position - (Vector2)handContainer.position);
            foreach (var hit in raycastHits)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<Enemy>().TakeDamage(kiteDamage);
                }
            }
        }

        public void ToggleKite(bool toggle)
        {
            if (!toggle)
            {
                transform.parent = handContainer;
                transform.localPosition = initialLocalPosition;
            }
            
            gameObject.SetActive(toggle);
        }
        
        public void Move(Vector3 newPosition)
        {
            if (transform.parent == handContainer)
            {
                transform.parent = projectileContainer;
            }
            
            startPosition = transform.position;
            endPosition = newPosition;
            isMovingToClick = true;
            kiteSpeedTimer = 0;
        }
    }
}