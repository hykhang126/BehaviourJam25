using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
        if (currentEnemyCount == -1 || currentEnemyCount < maxEnemies)
        {
            Instantiate(enemyPrefab, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].position, Quaternion.identity);
            currentEnemyCount = (currentEnemyCount == -1) ? 1 : currentEnemyCount + 1;
        }
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
