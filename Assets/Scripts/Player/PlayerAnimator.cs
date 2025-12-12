using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] idleSprites;
        [SerializeField] private Sprite[] leapSprites;
        [SerializeField] private Sprite[] deadSprites;
        [SerializeField] private PlayerController playerController;
        
        private GameManager gameManager;
        
        private Sprite idleSprite;
        private Sprite leapSprite;
        private Sprite deadSprite;
        
        public void Init(int playerIndex)
        {
            Debug.Log("Player Index: " + playerIndex);
            SetSprites(playerIndex);
        }
        
        private void SetSprites(int playerIndex)
        {
            spriteRenderer.sprite = idleSprites[playerIndex];
            
            idleSprite = idleSprites[playerIndex];
            leapSprite = leapSprites[playerIndex];
            deadSprite = deadSprites[playerIndex];
        }

        public void SetIdleSprite()
        {
            spriteRenderer.sprite = idleSprite;
        }

        public void SetLeapingSprite()
        {
            spriteRenderer.sprite = leapSprite;
        }

        public void SetDeadSprite()
        {
            spriteRenderer.sprite = deadSprite;
        }
    }
}
