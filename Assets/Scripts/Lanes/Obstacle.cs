using UnityEngine;

namespace Frogxel.Lanes
{
    public class Obstacle : MonoBehaviour
    {
        public const int Width = 1;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        public void SetSprite(Sprite sprite, bool flipX)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = flipX;
        }
        
        public void TrySetAnimatorController(RuntimeAnimatorController animatorController)
        {
            if (animatorController == null)
            {
                return;
            }
            
            animator.enabled = true;
            animator.runtimeAnimatorController = animatorController;
        }
    }
}