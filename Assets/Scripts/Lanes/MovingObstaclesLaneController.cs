using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingObstaclesLaneController : LaneController
    {
        [Header("Moving Lane")]
        [SerializeField] private ObstacleGroup obstacleGroupPrefab;

        private readonly List<ObstacleGroup> _obstacleGroups = new();
        private Vector2 _moveDirection;
        private float _moveSpeed;
        private float _minPosX;
        private float _maxPosX;
        private float _resetMinPosX;
        private float _resetMaxPosX;
        
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingObstaclesLaneConfig movingLaneConfig)
            {
                throw new Exception("Lane config is not a MovingLaneConfig");
            }
            
            var gapWidth = movingLaneConfig.GapWidth;
            
            _moveDirection = movingLaneConfig.MoveDirection;
            _moveSpeed = movingLaneConfig.MoveSpeed;
            _resetMinPosX = transform.position.x - width / 2f;
            _resetMaxPosX = transform.position.x + width / 2f;
            _minPosX = _resetMinPosX - gapWidth;
            _maxPosX = _resetMaxPosX + gapWidth;
            
            CreateObstacles(movingLaneConfig, width);
        }

        private void CreateObstacles(MovingObstaclesLaneConfig movingObstaclesLaneConfig, int width)
        {
            var totalObstaclesPerGroup = movingObstaclesLaneConfig.Count;
            var gapWidth = movingObstaclesLaneConfig.GapWidth;
            var obstacleGroupWidth = ObstacleGroup.GetObstacleGroupWidth(totalObstaclesPerGroup);
            var totalSpacePerObstacleGroup = obstacleGroupWidth + gapWidth;

            if (width < totalSpacePerObstacleGroup)
            {
                throw new Exception($"Not enough space to create obstacles. Need at least {totalSpacePerObstacleGroup}, but only have {width}");
            }
            
            var obstaclesCount = Mathf.CeilToInt(width * 1f / totalSpacePerObstacleGroup);
            var obstacleGroupPositionX = 0 - width / 2f + obstacleGroupWidth / 2f;
            var spaceBetweenObstacleGroups = obstacleGroupWidth + gapWidth;
            
            for (var i = 0; i < obstaclesCount; i++)
            {
                var obstacleGroup = Instantiate(obstacleGroupPrefab, transform, false);
                
                obstacleGroup.transform.localPosition = new Vector3(obstacleGroupPositionX, 0, 0);
                
                obstacleGroupPositionX += spaceBetweenObstacleGroups;

                obstacleGroup.Init(movingObstaclesLaneConfig);
                
                _obstacleGroups.Add(obstacleGroup);
            }
        }
        
        private void Update()
        {
            TryMoveObstacleGroups();
        }

        private void TryMoveObstacleGroups()
        {
            if (_obstacleGroups.Count <= 0)
            {
                return;
            }

            foreach (var obstacleGroup in _obstacleGroups)
            {
                obstacleGroup.Move(_moveDirection, _moveSpeed);
                obstacleGroup.TryResetPosition(_moveDirection, _resetMinPosX, _resetMaxPosX, _minPosX, _maxPosX);
            }
        }
    }
}