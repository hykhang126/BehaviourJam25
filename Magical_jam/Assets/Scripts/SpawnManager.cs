using System.Collections.Generic;
using Enemies;
using Levels;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private class LevelData
    {
        public LevelColor LevelColor;
        public List<Enemy> Enemies;
    }
    
    [SerializeField] private Level levelManager;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private float enemySpawnPointUpdateVectorScalar;
    [SerializeField] private Transform weaponProjectileContainer;
    [SerializeField] private List<SewerGrate> sewerGrates;
    [SerializeField] private SpawnPointsBound spawnPointsBound;
    
    private LevelColor currentLevelColor;
    private float lastEnemySpawnTime;
    private bool isInitialized;
    
    private readonly List<LevelData> levelData = new();

    public void Initialize()
    {
        isInitialized = true;

        if (spawnPointsBound.TopLeft == null || spawnPointsBound.BottomRight == null)
        {
            Debug.LogWarning("Top left and bottom right spawn points bounds are not set in the inspector.");
            spawnPointsBound.TopLeft = enemySpawnPoints[0];
            spawnPointsBound.BottomRight = enemySpawnPoints[^1];
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        var levelDataIndex = GetLevelDataIndex(currentLevelColor);
        if (levelDataIndex == -1 || levelData.Count <= levelDataIndex)
        {
            return;
        }
        
        Level.LevelData data = levelManager.GetLevelData(currentLevelColor);
        if (Time.time - lastEnemySpawnTime > data.SpawnCooldown && levelData[levelDataIndex].Enemies.Count < data.MaxEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyPrefab = levelManager.GetLevelData(currentLevelColor);

        // Get random spawn point and update its position for more randomness
        var spawnPointIndex = Random.Range(0, enemySpawnPoints.Count);
        var spawnPoint = (Vector2)enemySpawnPoints[spawnPointIndex].position;
        spawnPoint += new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * enemySpawnPointUpdateVectorScalar;
        // Clamp the spawn point between top left and bottom right
        spawnPoint.x = Mathf.Clamp(spawnPoint.x, spawnPointsBound.TopLeft.position.x, spawnPointsBound.BottomRight.position.x);
        spawnPoint.y = Mathf.Clamp(spawnPoint.y, spawnPointsBound.BottomRight.position.y, spawnPointsBound.TopLeft.position.y);
        // Check if spawn point is inside any collider, if yes then don't spawn enemy
        var colliders = Physics2D.OverlapCircleAll(spawnPoint, 0.5f);
        if (colliders.Length > 0)
            return;
            
        // Update the spawn point in the list to avoid spawning at the same location
        enemySpawnPoints[spawnPointIndex].position = spawnPoint;
        
        // Set up enemy
        var enemyGameObject = Instantiate(enemyPrefab.SpawnObject, spawnPoint, Quaternion.identity);
        var enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.Initialize(levelManager.Player, weaponProjectileContainer);
        enemy.transform.SetParent(enemyContainer);
        enemy.OnDeath += HandleEnemyDeath;
        lastEnemySpawnTime = Time.time;

        var levelColorIndex = GetLevelDataIndex(currentLevelColor);
        if (levelColorIndex != -1)
        {
            var enemies = levelData[levelColorIndex].Enemies;
            enemies.Add(enemy);
            levelData[levelColorIndex].Enemies = enemies;
        }
        else
        {
            var enemies = new List<Enemy>();
            enemies.Add(enemy);
            levelData.Add(new LevelData
            {
                LevelColor = currentLevelColor,
                Enemies = enemies
            });
        }
        
        // Ignore collisions with each sewer grate for blue enemies only
        if (enemy.LevelColor is not LevelColor.Blue)
        {
            return;
        }
        
        foreach (var sewerGrate in sewerGrates)
        {
            if (!sewerGrate)
            {
                continue;
            }
            
            Physics2D.IgnoreCollision(sewerGrate.SewerGrateCollider, enemy.EnemyCollider);
        }
    }

    private void HideCurrentEnemies()
    {
        var dataIndex = GetLevelDataIndex(currentLevelColor);
        if (dataIndex == -1)
        {
            return;
        }
        
        var enemies = levelData[dataIndex].Enemies;
        foreach (var enemy in enemies)
        {
            enemy.SetEnemyState(EnemyState.Dormant);
        }
    }

    private void ShowCurrentEnemies()
    {
        var dataIndex = GetLevelDataIndex(currentLevelColor);
        if (dataIndex == -1)
        {
            return;
        }
        
        var enemies = levelData[dataIndex].Enemies;
        foreach (var enemy in enemies)
        {
            enemy.SetEnemyState(EnemyState.Attacking);
        }
    }

    private int GetLevelDataIndex(LevelColor levelColor)
    {
        return levelData.FindIndex(x => x.LevelColor == levelColor);
    }

    public void SetLevelColor(LevelColor levelColor)
    {
        HideCurrentEnemies();
        
        currentLevelColor = levelColor;
        if (GetLevelDataIndex(currentLevelColor) == -1)
        {
            levelData.Add(new LevelData
            {
                LevelColor = levelColor,
                Enemies = new List<Enemy>()
            });
        }
        
        ShowCurrentEnemies();
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        var dataIndex = GetLevelDataIndex(currentLevelColor);
        var enemies = levelData[dataIndex].Enemies;
        enemies.Remove(enemy);
        levelData[dataIndex].Enemies = enemies;

        enemy.OnDeath -= HandleEnemyDeath;
    }
}

[System.Serializable]
public class SpawnPointsBound
{
    public Transform TopLeft;
    public Transform BottomRight;
}
