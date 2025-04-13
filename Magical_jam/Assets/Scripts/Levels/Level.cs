using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Levels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// SINGLETON

    [SerializeField] private LevelColorManager _levelColorManager;
    [SerializeField] public SpawnManager spawnManager;
    [SerializeField] private List<LevelData> enemyData;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    
    [SerializeField] private LevelColor currentColor;
    
    public Player Player => player;
    
    public Transform[] spawnPoints;

    public SpawnManager[] spawnManagers;
    

    public AudioClip[] bgmClips;

    private int _currentColorIndex = 0;

    private LevelColor _currentLevelColor;
    public LevelColor CurrentLevelColor
    {
        get => _currentLevelColor;
        set
        {
            _currentLevelColor = value;
        }
    }
    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentLevelColor = newColor;
        PlayAudio();
        currentColor = newColor;
    }

    public void Start()
    {
        _levelColorManager.OnLevelColorChanged.AddListener(HandleLevelColorChanged);
    
        _levelColorManager.Initialize();
        spawnManager.Initialize();
        
        SpawnPlayer();
    }

    private void OnDestroy()
    {
        spawnManagers[_currentColorIndex].gameObject.SetActive(false);
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
        // audioSource.clip = bgmClips[musicIndex];
        audioSource.resource = GetLevelData(currentColor).BackgroundMusic;

        // Plays the AudioClip assigned to the AudioSource
        if (audioSource.resource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip is assigned to the AudioSource!");
        }
    }

    private void HandleLevelColorChanged(LevelColor levelColor)
    {
        currentColor = levelColor;
        spawnManager.SetLevelColor(levelColor);
        PlayAudio();
    }
}
