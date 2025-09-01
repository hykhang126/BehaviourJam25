using System;
using System.Collections.Generic;
using UnityEngine;

using Characters;
using Combat;
using Levels;

public class Level : MonoBehaviour
{
    [Serializable]
    public struct LevelData
    {
        public LevelColor LevelColor;
        public Color HUDColor;
        public GameObject SpawnObject;
        public AudioClip BackgroundMusic;
        public float SpawnCooldown;
        public int MaxEnemies;
    }

    /// <summary>
    /// SINGLETON
    /// </summary>
    public static Level Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// SINGLETON

    [Header("Level Setup")]
    [SerializeField] private LevelColorManager _levelColorManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private List<LevelData> enemyData;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    [SerializeField] private LevelInfoSO levelInfo;
    public Player Player => player;
    public LevelColor currentColor;

    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        currentColor = newColor;
        spawnManager.SetLevelColor(newColor);

        PlayAudio();
    }

    private void OnDestroy()
    {
        spawnManager.gameObject.SetActive(false);
        _levelColorManager.OnLevelColorChanged.RemoveAllListeners();
    }

    public LevelData GetLevelData(LevelColor levelColor)
    {
        return enemyData.Find(x => x.LevelColor == levelColor);
    }

    private void SpawnPlayer()
    {
        Transform spawnPoint = playerSpawnPoint;
        player.transform.position = spawnPoint.position;
    }

    private void PlayAudio()
    {
        audioSource.clip = GetLevelData(currentColor).BackgroundMusic;

        // Plays the AudioClip assigned to the AudioSource
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip is assigned to the AudioSource!");
        }
    }

    public int GetNumberOfEnemiesDefeated(LevelColor color)
    {
        return color switch
        {
            LevelColor.Red => levelInfo.redEnemiesKillCount,
            LevelColor.Blue => levelInfo.blueEnemiesKillCount,
            LevelColor.Green => levelInfo.greenEnemiesKillCount,
            _ => levelInfo.totalEnemiesKillCount,
        };
    }

    public void UpdateEnemyKilledCount(LevelColor color)
    {
        // Update the SOs first
        switch (color)
        {
            case LevelColor.Red:
                levelInfo.redEnemiesKillCount++;
                break;
            case LevelColor.Blue:
                levelInfo.blueEnemiesKillCount++;
                break;
            case LevelColor.Green:
                levelInfo.greenEnemiesKillCount++;
                break;
            default:
                break;
        }
        levelInfo.totalEnemiesKillCount++;

        // Notify Upgrade manager about the enemy killed count update
        UpgradeManager.Instance.CheckEnemyKilledCount(color);
    }

    // Start
    public void Start()
    {
        _levelColorManager.Initialize();
        spawnManager.Initialize();

        SpawnPlayer();
    }
}
