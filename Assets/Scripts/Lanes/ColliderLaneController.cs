using System;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class ColliderLaneController : LaneController
    {
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not ColliderLaneConfig obstacleLaneConfig)
            {
                throw new Exception("Lane config is not an ObstacleLaneConfig");
            }

            var layer = LayerMask.NameToLayer(obstacleLaneConfig.Layer);

            if (layer < 0)
            {
                throw new Exception($"\"{obstacleLaneConfig.Layer}\" is not a valid layer");
            }

            gameObject.layer = layer;
        }
    }
}