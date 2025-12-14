using System;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingPlatformLaneController : MovingLaneController
    {
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingPlatformLaneConfig obstacleLaneConfig)
            {
                throw new Exception("Lane config is not an MovingPlatformLaneConfig");
            }

            var layer = LayerMask.NameToLayer(obstacleLaneConfig.Layer);

            if (layer < 0)
            {
                throw new Exception($"\"{obstacleLaneConfig.Layer}\" is not a valid layer");
            }

            gameObject.layer = layer;
        }
        
        protected override int GetMoveableWidth(MovingLaneConfig config)
        {
            var moveablePrefab = config.MoveablePrefab;
            
            if (moveablePrefab is not Platform platform)
            {
                throw new Exception("Instantiated moving lane object is not a platform");
            }

            return platform.Size;
        }
    }
}