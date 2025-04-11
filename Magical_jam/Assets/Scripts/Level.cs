using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform[] spawnPoints;

    public SpawnManager[] spawnManagers;

    public AudioClip[] bgmClips;
    private AudioSource audioSource;

    public GameObject player;

    public GameObject agentBK;

    private static int currentRoomIndex = 0;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentRoomIndex = 0;
        // Spawn the player at the first spawn point
        SpawnPlayer(currentRoomIndex);

        // Play the sound automatically at start (optional)
        PlayAudio(currentRoomIndex);
    }

    public void Update()
    {
        // Check if the player has reached the end of the level
        if (spawnManagers[currentRoomIndex].currentEnemyCount == spawnManagers[currentRoomIndex].maxEnemies - 1)
        {
            spawnManagers[currentRoomIndex].gameObject.SetActive(false);
            // Move to the next room
            currentRoomIndex++;
            if (currentRoomIndex >= spawnPoints.Length)
            {
                // End of the level
                Debug.Log("End of the level");
                return;
            }
            // Spawn the player at the next spawn point
            SpawnPlayer(currentRoomIndex);

            // Play the sound automatically at start (optional)
            PlayAudio(currentRoomIndex);
        }
    }

    // Go to a spawn point and spawn the player
    public void SpawnPlayer(int index)
    {
        Transform spawnPoint = spawnPoints[index];
        player.transform.position = spawnPoint.position;
        agentBK.transform.position = spawnPoint.position;
        currentRoomIndex = index;
        // set the spawner to be active
        spawnManagers[index].gameObject.SetActive(true);
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
