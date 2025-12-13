using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class LanesController : MonoBehaviour
    {
        [SerializeField] private LanesConfig lanesConfig;
        [SerializeField] private Transform lanesParent;
        [SerializeField] private Vector2 startingLanePosition;
        
        private readonly List<LaneController> _lanes = new();
        
        private Vector2 _currentLanePosition;

        public void Init()
        {
            _currentLanePosition = startingLanePosition;
            
            foreach (var config in lanesConfig.Lanes)
            {
                // TODO: Replace with object pooling
                var lane = Instantiate(config.Prefab, _currentLanePosition + new Vector2(0, config.Height / 2f), Quaternion.identity, lanesParent);
                
                lane.Init(config, lanesConfig.LaneWidth);
                
                _lanes.Add(lane);
                
                _currentLanePosition += new Vector2(0, config.Height);
            }
        }

        public void Clear()
        {
            foreach (var lane in _lanes)
            {
                Destroy(lane.gameObject);
            }
            
            _lanes.Clear();
        }
    }
}