using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class ObstacleGroup : Moveable
    {
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
                    
                obstaclePositionX += Obstacle.Width;
                
                _obstacles.Add(obstacle);
            }
        }
        
        protected override int GetWidth()
        {
            return _obstacles.Count * Obstacle.Width;
        }
        
        private static float GetStartingObstaclePositionX(int totalObstacles)
        {
            if (totalObstacles <= 1)
            {
                return 0;
            }

            return 0 - Obstacle.Width / 2f * (totalObstacles - 1);
        }

        private static bool IsMovingRight(Vector2 moveDirection)
        {
            return moveDirection.x > 0;
        }
        
        private Obstacle CreateObstacle(MovingObstaclesLaneConfig movingObstaclesLaneConfig, Transform parent)
        {
            // TODO: Replace with object pooling
            var obstacle = Instantiate(obstaclePrefab, parent);
            var flipX = !IsMovingRight(movingObstaclesLaneConfig.MoveDirection);
            
            obstacle.SetSprite(movingObstaclesLaneConfig.Sprite, flipX);
            obstacle.TrySetAnimatorController(movingObstaclesLaneConfig.AnimatorController);

            return obstacle;
        }
    }
}