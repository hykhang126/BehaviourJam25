using Combat;
using Levels;
using UnityEngine;

namespace Enemies
{
    public class RedEnemy : Enemy
    {
        [Header(nameof(RedEnemy))]
        [SerializeField] private GameObject lavaPoolPrefab;
        [SerializeField] private float lavaPoolCooldown;
        
        private float lastLavaPoolSpawnTime;

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();

            var trajectory = playerPosition - (Vector2)transform.position;
            enemyRigidbody.linearVelocity = trajectory * (Time.fixedDeltaTime * moveSpeed);
            
            SpawnLavaPool();
        }

        private void SpawnLavaPool()
        {
            if (!lavaPoolPrefab)
            {
                Debug.LogError("Lava pool prefab not set.");
                return;
            }

            if (!(Time.time - lastLavaPoolSpawnTime > lavaPoolCooldown))
            {
                return;
            }
            
            lastLavaPoolSpawnTime = Time.time;
            var lavaPool = Instantiate(lavaPoolPrefab, transform.position, transform.rotation);
            lavaPool.transform.SetParent(weaponProjectileContainer);
        }
    }
}