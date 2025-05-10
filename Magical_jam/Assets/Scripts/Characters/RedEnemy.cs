using System.Collections.Generic;
using Combat;
using Levels;
using UnityEngine;
using Utility;

namespace Characters
{
    public class RedEnemy : Enemy
    {
        [Header(nameof(RedEnemy))]
        [SerializeField] private GameObject lavaPoolPrefab;
        [SerializeField] private float lavaPoolCooldown;
        
        private float lastLavaPoolSpawnTime;
        private bool projectileToggle;

        private readonly List<LavaPool> lavaPools = new();

        protected override void ToggleProjectiles(bool toggle)
        {
            if (projectileToggle == toggle)
            {
                return;
            }
            
            projectileToggle = toggle;
            foreach (var lavaPool in lavaPools)
            {
                lavaPool.SetActive(toggle);
            }
        }

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();
            
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
            lavaPools.Add(lavaPool.GetComponent<LavaPool>());
        }
    }
}