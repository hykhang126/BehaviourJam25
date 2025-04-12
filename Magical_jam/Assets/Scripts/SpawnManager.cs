using System.Collections;
using System.Collections.Generic;
using Combat;
using Enemies;
using Levels;
using UnityEngine;
using Utility;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Level levelManager;
    [SerializeField] private List<SewerGrate> sewerGrates;
    [SerializeField] private Transform weaponProjectileContainer;
    
    public GameObject enemyPrefab; // Drag your enemy prefab here in the inspector
    public Transform[] enemySpawnPoints; // The point where the enemy will spawn
    public float spawnInterval; // Time between spawns
    public int maxEnemies; // Maximum number of enemies in the scene

    public int currentEnemyCount = -1;

    void Start()
    {
        currentEnemyCount = -1;
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount != -1 && currentEnemyCount >= maxEnemies)
        {
            return;
        }
        
        var enemy = Instantiate(enemyPrefab, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].position, Quaternion.identity).GetComponent<Enemy>();
        enemy.Initialize(levelManager.player, weaponProjectileContainer);
        currentEnemyCount = (currentEnemyCount == -1) ? 1 : currentEnemyCount + 1;
        
        // Ignore collisions with each sewer grate for blue enemies only
        if (enemy.LevelColor is not LevelColor.Blue)
        {
            return;
        }
        
        foreach (var sewerGrate in sewerGrates)
        {
            Physics2D.IgnoreCollision(sewerGrate.SewerGrateCollider, enemy.EnemyCollider);
        }
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
