﻿using System;
using System.Collections.Generic;
using TMPro;
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
        
        [SerializeField] private float _levelColorSwapCooldown = 5;
        [SerializeField] private bool _areLevelColorsRandomized;
        [SerializeField] private List<LevelColor> _levelColorOrder;
        [SerializeField] private int _minNumberOfLevelsLoaded = 3;
        [SerializeField] private TMP_Text _textTest;
        
        private int _currentLevelDataIndex;
        
        private readonly List<LevelData> _levelData = new();
        private readonly Queue<LevelColor> _upcomingLevelColors = new();

        private void Start()
        {
            _levelData.Clear();
            _upcomingLevelColors.Clear();
            _textTest.text = "";
            
            AddUpcomingLevels(_minNumberOfLevelsLoaded);
            GoToNextLevel();
        }
        
        private void Update()
        {
            if (Time.time - _levelData[_currentLevelDataIndex].LastLevelStartTime >=
                _levelColorSwapCooldown)
            {
                GoToNextLevel();
                AddUpcomingLevel();
                _textTest.text = _textTest.text + " " + _levelData[_currentLevelDataIndex].LevelColor;
            }
        }
        
        private void AddUpcomingLevels(int numberOfLevelsToLoad)
        {
            for (var i = 0; i < numberOfLevelsToLoad; i++)
            {
                AddUpcomingLevel();
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
            _currentLevelDataIndex = nextLevelDataIndex;

            OnLevelColorChanged?.Invoke(nextLevelColor);
            
            if (nextLevelDataIndex == -1)
            {
                _levelData.Add(new LevelData
                {
                    LevelColor = nextLevelColor,
                    LastLevelStartTime = Time.time,
                });
                _currentLevelDataIndex = _levelData.Count - 1;
                return;
            }
            
            var nextLevelDataToUpdate = _levelData[nextLevelDataIndex];
            nextLevelDataToUpdate.LastLevelStartTime = Time.time;
            _levelData[nextLevelDataIndex] = nextLevelDataToUpdate;
        }
    }
}