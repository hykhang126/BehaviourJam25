using UnityEngine;

using Levels;

[CreateAssetMenu(fileName = "LevelInfoSO", menuName = "Scriptable Objects/LevelInfoSO")]
public class LevelInfoSO : ScriptableObject
{
    public int redEnemiesKillCount;
    public int blueEnemiesKillCount;
    public int greenEnemiesKillCount;

    public int totalEnemiesKillCount;

    [NaughtyAttributes.Button]
    private void ClearData()
    {
        redEnemiesKillCount = blueEnemiesKillCount = greenEnemiesKillCount = totalEnemiesKillCount = 0;
    }

}
