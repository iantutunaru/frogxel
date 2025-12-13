using System;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingObstaclesLaneController : LaneController
    {
        private const int ObstacleWidth = 1;
        
        [Header("Moving Lane")]
        [SerializeField] private Obstacle obstaclePrefab;
        
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingObstaclesLaneConfig movingLaneConfig)
            {
                throw new Exception("Lane config is not a MovingLaneConfig");
            }

            CreateObstacles(movingLaneConfig, width);
        }
        
        private static float GetStartingObstaclePositionX(int totalObstacles)
        {
            if (totalObstacles <= 1)
            {
                return 0;
            }

            return 0 - ObstacleWidth / 2f * (totalObstacles - 1);
        }

        private void CreateObstacles(MovingObstaclesLaneConfig movingObstaclesLaneConfig, int width)
        {
            var totalObstaclesPerContainer = movingObstaclesLaneConfig.Count;
            var gapWidth = movingObstaclesLaneConfig.GapWidth;
            var obstacleContainerWidth = totalObstaclesPerContainer * ObstacleWidth;
            var totalSpacePerObstacleContainer = obstacleContainerWidth + gapWidth;

            if (width < totalSpacePerObstacleContainer)
            {
                throw new Exception($"Not enough space to create obstacles. Need at least {totalSpacePerObstacleContainer}, but only have {width}");
            }
            
            var obstaclesCount = Mathf.CeilToInt(width * 1f / totalSpacePerObstacleContainer);
            var obstacleContainerPositionX = 0 - width / 2f + obstacleContainerWidth / 2f;
            var spaceBetweenObstacleContainers = obstacleContainerWidth + gapWidth;
            
            for (var i = 0; i < obstaclesCount; i++)
            {
                var obstacleContainer = new GameObject($"ObstacleContainer_{i}");
                var obstacleContainerTransform = obstacleContainer.transform;

                obstacleContainerTransform.SetParent(transform, false);
                
                obstacleContainerTransform.localPosition = new Vector3(obstacleContainerPositionX, 0, 0);

                obstacleContainerPositionX += spaceBetweenObstacleContainers;
                
                var obstaclePositionX = GetStartingObstaclePositionX(totalObstaclesPerContainer);

                for (var j = 0; j < totalObstaclesPerContainer; j++)
                {
                    var obstacle = CreateObstacle(movingObstaclesLaneConfig, obstacleContainerTransform);
                    
                    obstacle.transform.localPosition = new Vector3(obstaclePositionX, 0, 0);
                    
                    obstaclePositionX += ObstacleWidth;
                }
            }
        }

        private Obstacle CreateObstacle(MovingObstaclesLaneConfig movingObstaclesLaneConfig, Transform parent)
        {
            // TODO: Replace with object pooling
            var obstacle = Instantiate(obstaclePrefab, parent);
            
            obstacle.SetSprite(movingObstaclesLaneConfig.Sprite);
            obstacle.TrySetAnimatorController(movingObstaclesLaneConfig.AnimatorController);

            return obstacle;
        }
    }
}