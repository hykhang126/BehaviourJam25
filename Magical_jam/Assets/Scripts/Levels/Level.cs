using System.Collections;
using System.Collections.Generic;
using Combat;
using Levels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
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
    
    public Transform[] spawnPoints;

    public SpawnManager[] spawnManagers;

    public AudioClip[] bgmClips;
    private AudioSource audioSource;

    public Player player;

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
    public void UpdatePlayerColor(LevelColor newColor)
    {
        _currentLevelColor = newColor;
    }

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _currentColorIndex = 0;
        // Spawn the player at the first spawn point
        SpawnPlayer(_currentColorIndex);

        // Play the sound automatically at start (optional)
        PlayAudio(_currentColorIndex);
    }

    public void Update()
    {
        // Check if the player has reached the end of the level
        if (spawnManagers.Length > 0 && spawnManagers[_currentColorIndex].currentEnemyCount == spawnManagers[_currentColorIndex].maxEnemies)
        {
            spawnManagers[_currentColorIndex].gameObject.SetActive(false);
            // NEVER Move to the next room
            // currentRoomIndex++;
            if (_currentColorIndex >= spawnPoints.Length)
            {
                // End of the level
                Debug.Log("End of the level");
                return;
            }

            // Play the sound automatically at start (optional)
            PlayAudio(_currentColorIndex);
        }
    }

    // Go to a spawn point and spawn the player
    public void SpawnPlayer(int index)
    {
        Transform spawnPoint = spawnPoints[index];
        player.transform.position = spawnPoint.position;
    }

    public void PlayAudio(int index)
    {
        audioSource.clip = bgmClips[index];

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
}
