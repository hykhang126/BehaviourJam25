using System.Collections.Generic;
using Characters;
using Combat;
using Levels;
using UnityEngine;

public class Level : MonoBehaviour
{
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
    public SpawnManager spawnManager;
    [SerializeField] private List<LevelData> enemyData;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    public Player Player => player;
    private LevelColor currentColor;

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
    
    // Start
    public void Start()
    {
        _levelColorManager.Initialize();
        spawnManager.Initialize();

        SpawnPlayer();
    }
}
