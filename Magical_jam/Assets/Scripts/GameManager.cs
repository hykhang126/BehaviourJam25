using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            player = FindAnyObjectByType(typeof(Player)) as Player;
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps the GameManager alive between scenes
        }
        else
        {
            Destroy(gameObject);  // Prevents duplicates
        }
    }
}