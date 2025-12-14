using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Leap = Animator.StringToHash("Leap");
        private static readonly int Death = Animator.StringToHash("Die");
        
        //[SerializeField] private SpriteRenderer spriteRenderer;
        //[SerializeField] private Sprite[] idleSprites;
        //[SerializeField] private Sprite[] leapSprites;
        //[SerializeField] private Sprite[] deadSprites;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator animator;
        
        private GameManager gameManager;
        
        // private Sprite idleSprite;
        // private Sprite leapSprite;
        // private Sprite deadSprite;
        
        // public void Init(int playerIndex)
        // {
        //     //Debug.Log("Player Index: " + playerIndex);
        //     //SetSprites(playerIndex);
        // }
        
        // private void SetSprites(int playerIndex)
        // {
        //     spriteRenderer.sprite = idleSprites[playerIndex];
        //     
        //     idleSprite = idleSprites[playerIndex];
        //     leapSprite = leapSprites[playerIndex];
        //     deadSprite = deadSprites[playerIndex];
        // }

        public void SetIdleSprite()
        {
            animator.SetBool(Leap, false);
            //animator.SetBool(Death, false);
            // spriteRenderer.sprite = idleSprite;
        }

        public void ResetAnimator()
        {
            animator.Rebind();
            animator.Update(0f);
        }

        public void SetLeapingSprite()
        {
            animator.SetBool(Leap, true);
            //spriteRenderer.sprite = leapSprite;
        }

        public void SetDeadSprite()
        {
            animator.SetBool(Death, true);
            //.sprite = deadSprite;
        }
    }
}
