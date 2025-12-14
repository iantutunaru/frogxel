using UnityEngine;

namespace Frogxel.Lanes
{
    public class Moveable : MonoBehaviour
    {
        public void Move(Vector2 direction, float moveSpeed)
        {
            transform.Translate(direction * (moveSpeed * Time.deltaTime));
        }

        public void TryResetPosition(Vector2 moveDirection, Vector2 firstObjectPosition, Vector2 lastObjectPosition)
        {
            var currentPosition = transform.localPosition;
            var currentPosX = currentPosition.x;
            var isMovingRight = moveDirection.x > 0;
            var lastObjectPosX = lastObjectPosition.x;

            if (isMovingRight)
            {
                if (currentPosX > lastObjectPosX)
                {
                    transform.localPosition = firstObjectPosition;
                }
                
                return;
            }
            
            if (currentPosX >= lastObjectPosX)
            {
                return;
            }
            
            transform.localPosition = firstObjectPosition;
        }
    }
}