using UnityEngine;
using System.Collections;

using Characters;
using Combat;
using Levels;

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
    public Transform upgradePointsUI;
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

        // Null checks
        if (!upgradePointsUI)
        {
            Debug.LogError("Upgrade Points UI is not assigned in the UpgradeManager.");
        }
    }
    /// SINGLETON

    private IEnumerator ShowUpgradePointsUICoroutine(Transform uiElement, float duration = 3f)
    {
        // Move the UI element to the player's position
        uiElement.gameObject.SetActive(true);

        // Wait for a short duration
        yield return new WaitForSeconds(duration);

        // Move the UI element back to its original position
        uiElement.gameObject.SetActive(false);
    }
    public void AwardUpgradePoint(LevelColor color)
    {
        switch (color)
        {
            case LevelColor.Red:
                redUpgradePoints++;
                break;
            case LevelColor.Blue:
                blueUpgradePoints++;
                break;
            case LevelColor.Green:
                greenUpgradePoints++;
                break;
        }

        // Start coutine to show upgrade points UI, then move it back to original position
        float waitTime = 3f;
        StartCoroutine(ShowUpgradePointsUICoroutine(upgradePointsUI, waitTime));
    }

    public void CheckEnemyKilledCount(LevelColor color)
    {
        switch (color)
        {
            case LevelColor.Red:
                redDifficulty.CheckIfShouldIncreaseDifficultyStage(color);
                break;
            case LevelColor.Blue:
                blueDifficulty.CheckIfShouldIncreaseDifficultyStage(color);
                break;
            case LevelColor.Green:
                greenDifficulty.CheckIfShouldIncreaseDifficultyStage(color);
                break;
            default:
                Debug.LogWarning("Unknown level color: " + color);
                break;
        }
    }

    void Start()
    {
        redDifficulty.Initialize();
        blueDifficulty.Initialize();
        greenDifficulty.Initialize();
    }
}