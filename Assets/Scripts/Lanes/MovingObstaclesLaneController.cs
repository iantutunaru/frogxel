using System;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingObstaclesLaneController : LaneController
    {
        private const int ObstacleWidth = 1;
        
        [Header("Moving Lane")]
        [SerializeField] private ObstacleGroup obstacleGroupPrefab;
        
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingObstaclesLaneConfig movingLaneConfig)
            {
                throw new Exception("Lane config is not a MovingLaneConfig");
            }

            CreateObstacles(movingLaneConfig, width);
        }

        private void CreateObstacles(MovingObstaclesLaneConfig movingObstaclesLaneConfig, int width)
        {
            var totalObstaclesPerGroup = movingObstaclesLaneConfig.Count;
            var gapWidth = movingObstaclesLaneConfig.GapWidth;
            var obstacleGroupWidth = totalObstaclesPerGroup * ObstacleWidth;
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
            }
        }
    }
}