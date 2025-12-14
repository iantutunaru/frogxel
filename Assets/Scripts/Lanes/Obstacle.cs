using UnityEditor.Animations;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        public void SetSprite(Sprite sprite, bool flipX)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = flipX;
        }
        
        public void TrySetAnimatorController(AnimatorController animatorController)
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