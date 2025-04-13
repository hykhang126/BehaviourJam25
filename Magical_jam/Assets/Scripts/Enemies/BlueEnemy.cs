using System.Threading;
using Combat;
using Levels;
using UnityEngine;
using Core;

namespace Enemies
{
    public class BlueEnemy : Enemy
    {
        [Header(nameof(BlueEnemy))]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileCooldown = 3f;

        private Core.Timer timer;

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();

            var trajectory = playerPosition - (Vector2)transform.position;
            trajectory.Normalize();
            enemyRigidbody.linearVelocity = trajectory * (Time.fixedDeltaTime * moveSpeed);

            SpawnRainDropProjectile();
        }

        private void SpawnRainDropProjectile()
        {
            if (timer == null || timer.time <= 0f)
            {
                timer = new Core.Timer(projectileCooldown);
                var rainDrop = Instantiate(projectilePrefab, transform.position, transform.rotation);
                rainDrop.transform.SetParent(weaponProjectileContainer);
                
                Vector2 trajectory = playerPosition - (Vector2)transform.position;
                trajectory.Normalize();
                rainDrop.GetComponent<Bullet>().Initialize(trajectory, gameObject.name, 200f);
            }
            timer.Tick(Time.fixedDeltaTime);
        }
    }
}