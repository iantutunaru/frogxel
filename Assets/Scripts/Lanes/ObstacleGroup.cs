using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class ObstacleGroup : MonoBehaviour
    {
        private const int ObstacleWidth = 1;
        
        [SerializeField] private Obstacle obstaclePrefab;

        private readonly List<Obstacle> _obstacles = new();

        public void Init(MovingObstaclesLaneConfig movingObstaclesLaneConfig)
        {
            var totalObstacles = movingObstaclesLaneConfig.Count;
            var obstaclePositionX = GetStartingObstaclePositionX(totalObstacles);

            for (var i = 0; i < totalObstacles; i++)
            {
                var obstacle = CreateObstacle(movingObstaclesLaneConfig, transform);
                    
                obstacle.transform.localPosition = new Vector3(obstaclePositionX, 0, 0);
                    
                obstaclePositionX += ObstacleWidth;
                
                _obstacles.Add(obstacle);
            }
        }
        
        private static float GetStartingObstaclePositionX(int totalObstacles)
        {
            if (totalObstacles <= 1)
            {
                return 0;
            }

            return 0 - ObstacleWidth / 2f * (totalObstacles - 1);
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