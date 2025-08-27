using UnityEngine;

using Characters;
using Combat;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Manager Setup")]
    public static UpgradeManager Instance { get; private set; }
    public Player player;

    [Header("Color Difficulty")]
    public RedDifficulty redDifficulty;
    public BlueDifficulty blueDifficulty;
    public GreenDifficulty greenDifficulty;

    [Header("Upgrade points")]
    public int redUpgradePoints;
    public int blueUpgradePoints;
    public int greenUpgradePoints;

    /// <summary>
    /// SINGLETON
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            if (!player) player = FindAnyObjectByType(typeof(Player)) as Player;
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Prevents duplicates
        }
    }
    /// SINGLETON

    void Start()
    {
        redDifficulty = new RedDifficulty();
        blueDifficulty = new BlueDifficulty();
        greenDifficulty = new GreenDifficulty();
        redDifficulty.Initialize();
        blueDifficulty.Initialize();
        greenDifficulty.Initialize();
    }
}