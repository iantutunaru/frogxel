using UnityEngine;

namespace Frogxel.Lanes
{
    public class Moveable : MonoBehaviour
    {
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
            var width = GetWidth();
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

        protected virtual int GetWidth()
        {
            return 0;
        }

        private static bool IsMovingRight(Vector2 moveDirection)
        {
            return moveDirection.x > 0;
        }
    }
}