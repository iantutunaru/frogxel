using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class CapturePointsLaneController : LaneController
    {
        public static event Action<List<CapturePoint>> CapturePointsInitialised;
        
        [SerializeField] private List<CapturePoint> points;

        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);
            
            CapturePointsInitialised?.Invoke(points);
        }
    }
}