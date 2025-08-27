using System;
using UnityEngine;

namespace Combat
{
    public abstract class Difficulty
    {
        [SerializeField] protected int enemyCount;
        [SerializeField] protected float spawnRate;
        [SerializeField] protected float playerDamage;
        [SerializeField] protected int[] numberOfEnemiesToDefeatPerDifficultyStage;
        [SerializeField] protected int currentStageDifficultyIndex;

        public void IncreaseDifficultyStage()
        {
            if (currentStageDifficultyIndex < numberOfEnemiesToDefeatPerDifficultyStage.Length - 1)
            {
                currentStageDifficultyIndex++;
                // x1.5 enemyCCount, spawnRate & playerDamage
                enemyCount = Mathf.RoundToInt(enemyCount * 1.5f);
                spawnRate *= 1.5f;
                playerDamage *= 1.5f;
            }
        }

        public abstract void Initialize();
    }

    [Serializable]
    public class RedDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCount = 5;
            spawnRate = 1.5f;
            playerDamage = 10f;
        }
    }

    [Serializable]
    public class BlueDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCount = 7;
            spawnRate = 1.2f;
            playerDamage = 12f;
        }
    }

    [Serializable]
    public class GreenDifficulty : Difficulty
    {
        public override void Initialize()
        {
            enemyCount = 10;
            spawnRate = 1.0f;
            playerDamage = 15f;
        }
    }
}