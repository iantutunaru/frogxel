using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingLaneController : LaneController
    {
        private readonly List<Moveable> _moveableObjects = new();
        private Vector2 _moveDirection;
        private float _moveSpeed;
        private float _minPosX;
        private float _maxPosX;
        private float _resetMinPosX;
        private float _resetMaxPosX;
        
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingLaneConfig movingLaneConfig)
            {
                throw new Exception("Lane config is not a MovingLaneConfig");
            }
            
            var gapWidth = movingLaneConfig.GapWidth;
            
            _moveDirection = movingLaneConfig.MoveDirection;
            _moveSpeed = movingLaneConfig.MoveSpeed;
            _resetMinPosX = transform.position.x - width / 2f;
            _resetMaxPosX = transform.position.x + width / 2f;
            _minPosX = _resetMinPosX - gapWidth;
            _maxPosX = _resetMaxPosX + gapWidth;
            
            CreateObstacles(movingLaneConfig, width);
        }

        protected virtual int GetMoveableWidth(MovingLaneConfig config)
        {
            return 0;
        }

        protected virtual void HandleMoveableInstantiation(Moveable moveable, MovingLaneConfig config)
        {
        }

        private void CreateObstacles(MovingLaneConfig movingObstaclesLaneConfig, int width)
        {
            var gapWidth = movingObstaclesLaneConfig.GapWidth;
            var moveableWidth = GetMoveableWidth(movingObstaclesLaneConfig);
            var totalSpacePerMoveable = moveableWidth + gapWidth;

            if (width < totalSpacePerMoveable)
            {
                throw new Exception($"Not enough space to create moving objects. Need at least {totalSpacePerMoveable}, but only have {width}");
            }
            
            var totalMovingObjects = Mathf.CeilToInt(width * 1f / totalSpacePerMoveable);
            var moveablePosX = 0 - width / 2f + moveableWidth / 2f;
            var spaceBetweenObjects = moveableWidth + gapWidth;
            
            for (var i = 0; i < totalMovingObjects; i++)
            {
                var moveable = Instantiate(movingObstaclesLaneConfig.MoveablePrefab, transform, false);
                
                moveable.transform.localPosition = new Vector3(moveablePosX, 0, 0);
                
                moveablePosX += spaceBetweenObjects;

                HandleMoveableInstantiation(moveable, movingObstaclesLaneConfig);
                
                _moveableObjects.Add(moveable);
            }
        }
        
        private void Update()
        {
            TryMoveObjects();
        }

        private void TryMoveObjects()
        {
            if (_moveableObjects.Count <= 0)
            {
                return;
            }

            foreach (var moveable in _moveableObjects)
            {
                moveable.Move(_moveDirection, _moveSpeed);
                moveable.TryResetPosition(_moveDirection, _resetMinPosX, _resetMaxPosX, _minPosX, _maxPosX);
            }
        }
    }
}