using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class ObstacleGroup : MonoBehaviour
    {
        private const int ObstacleWidth = 1;
        
        [SerializeField] private Obstacle obstaclePrefab;

        private readonly List<Obstacle> _obstacles = new();

        public static int GetObstacleGroupWidth(int totalObstacles)
        {
            return totalObstacles * ObstacleWidth;
        }

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

        public void Move(Vector2 direction, float moveSpeed)
        {
            transform.Translate(direction * (moveSpeed * Time.deltaTime));
        }

        public void TryResetPosition(Vector2 moveDirection, float minResetPosX, float maxResetPosX, float minPosX,
            float maxPosX)
        {
            var currentPosition = transform.position;
            var currentPosX = currentPosition.x;
            var currentPosY = currentPosition.y;
            var isMovingRight = IsMovingRight(moveDirection);
            var width = GetObstacleGroupWidth(_obstacles.Count);
            var halfWidth = width / 2f;

            if (isMovingRight)
            {
                if (currentPosX >= maxPosX - halfWidth)
                {
                    transform.position = new Vector2(minResetPosX - halfWidth, currentPosY);
                }
                
                return;
            }
            
            if (currentPosX > minPosX + halfWidth)
            {
                return;
            }
            
            transform.position = new Vector2(maxResetPosX + halfWidth, currentPosY);
        }
        
        private static float GetStartingObstaclePositionX(int totalObstacles)
        {
            if (totalObstacles <= 1)
            {
                return 0;
            }

            return 0 - ObstacleWidth / 2f * (totalObstacles - 1);
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