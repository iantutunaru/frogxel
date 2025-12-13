using UnityEditor.Animations;
using UnityEngine;

namespace Frogxel.Lanes
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
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