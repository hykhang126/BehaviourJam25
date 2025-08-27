using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Combat
{
    public class LevelColorManager : MonoBehaviour
    {
        
        /// <summary>
        /// COLOR CHANGED EVENT
        /// </summary>
        public UnityEvent<LevelColor> OnLevelColorChanged;
        //----------------------------------------------------------

        private struct LevelData
        {
            public LevelColor LevelColor;
            public float LastLevelStartTime;
        }
        
        public float levelColorSwapCooldown = 5;
        [SerializeField] private bool _areLevelColorsRandomized;
        [SerializeField] private List<LevelColor> _levelColorOrder;
        
        private int _currentLevelDataIndex;
        
        private readonly List<LevelData> _levelData = new();
        private readonly Queue<LevelColor> _upcomingLevelColors = new();

        public void Initialize()
        {
            _levelData.Clear();
            _upcomingLevelColors.Clear();
            
            AddInitialLevelsBasedOnOrder(1);
            GoToNextLevel();
        }
        
        private void Update()
        {
            if (_levelData.Count == 0)
            {
                return;
            }
            
            if (Time.time - _levelData[_currentLevelDataIndex].LastLevelStartTime >=
                levelColorSwapCooldown)
            {
                AddUpcomingLevel();
                GoToNextLevel();
            }
        }
        
        private void AddInitialLevelsBasedOnOrder(int numberOfLevelsToLoad)
        {
            for (var i = 0; i < numberOfLevelsToLoad; i++)
            {
                LevelColor levelToAdd = _levelColorOrder[i % _levelColorOrder.Count];
                _upcomingLevelColors.Enqueue(levelToAdd);
            }
        }

        private void AddUpcomingLevel()
        {
            _upcomingLevelColors.Enqueue(GetNextLevelColor());
        }
        
        private LevelColor GetNextLevelColor()
        {
            var currentLevelColor = _currentLevelDataIndex < _levelData.Count ? _levelData[_currentLevelDataIndex].LevelColor : LevelColor.None;
            var nextIndex = 0;
            
            if (_areLevelColorsRandomized)
            {
                var random = new Random();
                nextIndex = random.Next(_levelColorOrder.Count);
            }
            else
            {
                var currentIndex = _levelColorOrder.IndexOf(currentLevelColor);
                nextIndex = (currentIndex + 1) % _levelColorOrder.Count;
            }
            
            return _levelColorOrder[nextIndex];
        }

        private void GoToNextLevel()
        {
            var nextLevelColor = _upcomingLevelColors.Dequeue();
            var nextLevelDataIndex = _levelData.FindIndex(x => x.LevelColor == nextLevelColor);

            if (_currentLevelDataIndex == nextLevelDataIndex)
                return;

            _currentLevelDataIndex = nextLevelDataIndex;
            
            if (nextLevelDataIndex == -1)
            {
                _levelData.Add(new LevelData
                {
                    LevelColor = nextLevelColor,
                    LastLevelStartTime = Time.time,
                });
                _currentLevelDataIndex = _levelData.Count - 1;
                // Update event
                OnLevelColorChanged?.Invoke(nextLevelColor);
                return;
            }

            // Update event
            OnLevelColorChanged?.Invoke(nextLevelColor);
            
            var nextLevelDataToUpdate = _levelData[nextLevelDataIndex];
            nextLevelDataToUpdate.LastLevelStartTime = Time.time;
            _levelData[nextLevelDataIndex] = nextLevelDataToUpdate;
        }
    }
}