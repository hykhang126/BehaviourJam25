using System;
using UnityEngine;

using Levels;

namespace Combat
{
    public abstract class Difficulty
    {
        [SerializeField] protected float enemyCountMultiplier;
        [SerializeField] protected float spawnRateMultiplier;
        [SerializeField] protected float playerDamageMultiplier;
        [SerializeField] protected int[] numberOfEnemiesToDefeatPerDifficultyStage;
        [SerializeField] protected int currentStageDifficultyIndex;

        public bool CheckIfShouldIncreaseDifficultyStage(LevelColor color)
        {
            if (Level.Instance.GetNumberOfEnemiesDefeated(color)
                >= numberOfEnemiesToDefeatPerDifficultyStage[currentStageDifficultyIndex])
            {
                IncreaseDifficultyStage(color);
                return true;
            }

            return false;
        }

        private void IncreaseDifficultyStage(LevelColor color)
        {
            if (currentStageDifficultyIndex < numberOfEnemiesToDefeatPerDifficultyStage.Length - 1)
            {
                currentStageDifficultyIndex++;
                // x1.5 enemyCount, spawnRate & playerDamage
                enemyCountMultiplier *= 1.5f;
                spawnRateMultiplier *= 1.5f;
                playerDamageMultiplier *= 1.5f;

                // GIVE PLAYER UPGRADE POINT WHEN DIFFICULTY CHANGES
                UpgradeManager.Instance.AwardUpgradePoint(color);
            }
        }

        public abstract void Initialize();
    }

    [Serializable]
    public class RedDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCountMultiplier = 1f;
            spawnRateMultiplier = 1f;
            playerDamageMultiplier = 1f;
        }
    }

    [Serializable]
    public class BlueDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCountMultiplier = 1f;
            spawnRateMultiplier = 1f;
            playerDamageMultiplier = 1f;
        }
    }

    [Serializable]
    public class GreenDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCountMultiplier = 1f;
            spawnRateMultiplier = 1f;
            playerDamageMultiplier = 1f;
        }
    }
}