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
        private Vector2 _firstObjectPos;
        private Vector2 _lastObjectPos;
        
        public override void Init(LaneConfig laneConfig, int width)
        {
            base.Init(laneConfig, width);

            if (laneConfig is not MovingLaneConfig movingLaneConfig)
            {
                throw new Exception("Lane config is not a MovingLaneConfig");
            }
            
            _moveDirection = movingLaneConfig.MoveDirection;
            _moveSpeed = movingLaneConfig.MoveSpeed;
            
            SetFirstObjectPosition(movingLaneConfig, width);
            CreateObjects(movingLaneConfig, width);
        }

        protected virtual int GetMoveableWidth(MovingLaneConfig config)
        {
            return 0;
        }

        protected virtual void HandleMoveableInstantiation(Moveable moveable, MovingLaneConfig config)
        {
        }

        private void CreateObjects(MovingLaneConfig config, int width)
        {
            var currentObjectPosition = _firstObjectPos;

            while (IsObjectPositionValid(currentObjectPosition, config, width))
            {
                var moveable = Instantiate(config.MoveablePrefab, transform, false);

                moveable.transform.localPosition = currentObjectPosition;
                
                HandleMoveableInstantiation(moveable, config);
                
                _moveableObjects.Add(moveable);
                
                currentObjectPosition = GetNextObjectPosition(currentObjectPosition, config);
            }
            
            _lastObjectPos = currentObjectPosition;
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
                moveable.TryResetPosition(_moveDirection, _firstObjectPos, _lastObjectPos);
            }
        }

        private void SetFirstObjectPosition(MovingLaneConfig config, int laneWidth)
        {
            var laneCentrePosX = transform.position.x;
            var halfLaneWidth = laneWidth / 2f;
            var moveableWidth = GetMoveableWidth(config);
            var halfMoveableWidth = moveableWidth / 2f;

            if (config.MoveDirection.x > 0)
            {
                var minLanePosX = laneCentrePosX - halfLaneWidth;
                
                _firstObjectPos = new Vector2(minLanePosX - halfMoveableWidth, 0);
                
                return;
            }
            
            var maxLanePosX = laneCentrePosX + halfLaneWidth;
            
            _firstObjectPos = new Vector2(maxLanePosX + halfMoveableWidth, 0);
        }

        private Vector2 GetNextObjectPosition(Vector2 previousObjectPos, MovingLaneConfig config)
        {
            var previousObjectPosX = previousObjectPos.x;
            var moveableWidth = GetMoveableWidth(config);
            var gapWidth = config.GapWidth;

            if (config.MoveDirection.x > 0)
            {
                return new Vector2(previousObjectPosX + moveableWidth + gapWidth, previousObjectPos.y);
            }
            
            return new Vector2(previousObjectPosX - moveableWidth - gapWidth, previousObjectPos.y);
        }

        private bool IsObjectPositionValid(Vector2 position, MovingLaneConfig config, int laneWidth)
        {
            var laneCentrePosX = transform.position.x;
            var halfLaneWidth = laneWidth / 2f;
            
            if (config.MoveDirection.x > 0)
            {
                var maxLanePosX = laneCentrePosX + halfLaneWidth;

                return position.x <= maxLanePosX;
            }
            
            var minLanePosX = laneCentrePosX - halfLaneWidth;
            
            return position.x >= minLanePosX;
        }
    }
}