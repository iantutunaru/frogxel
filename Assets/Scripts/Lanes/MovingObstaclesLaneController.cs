using System;

namespace Frogxel.Lanes
{
    public class MovingObstaclesLaneController : MovingLaneController
    {
        protected override int GetMoveableWidth(MovingLaneConfig config)
        {
            if (config is not MovingObstaclesLaneConfig movingObstaclesLaneConfig)
            {
                throw new Exception("Moving lane config is not a MovingObstaclesLaneConfig");
            }

            return movingObstaclesLaneConfig.Count * Obstacle.Width;
        }

        protected override void HandleMoveableInstantiation(Moveable moveable, MovingLaneConfig config)
        {
            if (config is not MovingObstaclesLaneConfig movingObstaclesLaneConfig)
            {
                throw new Exception("Moving lane config is not a MovingObstaclesLaneConfig");
            }

            if (moveable is not ObstacleGroup obstacleGroup)
            {
                throw new Exception("Instantiated moving lane object is not an obstacle group");
            }

            obstacleGroup.Init(movingObstaclesLaneConfig);
        }
    }
}