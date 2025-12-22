using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// 2D player controller that handles user input, movement, animations and death/respawn mechanics
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public static event Action Pause;
        public static event Action Resume;
        
        // Handles the player's visual representation in the game
        private SpriteRenderer spriteRenderer;
        // The position where the player character respawns
        private Vector3 spawnPosition;
        // Tracks the furthest row the player has reached
        private float farthestRow;
        // Indicates if the player is currently leaping
        private bool leaping;
        // Tracks if a key was released, used for input handling
        private bool keyReleased = true;
        // Initialize movement vector
        private Vector2 movementInput = Vector2.zero;
        // Bool representing if the player has just captured a home or the game has ended
        private bool won;
        // Action to continue the game after it has ended
        private InputAction menuJoin;
        
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private PlayerAnimator playerAnimator;
        
        [SerializeField] private float maxLeapDuration = 0.125f;
        [SerializeField] private string gameActionMapName;
        [SerializeField] private string uiActionMapName;
        
        private GameManager gameManager;
        private ScoreManager scoreManager;
        private PauseManager pauseManager;

        private const string StringInputActionMenuJoin = "MenuJoin";
        private const string BarrierLayerName = "Barrier";
        private const string PlatformLayerName = "Platform";
        private const string ObstacleLayerName = "Obstacle";
        private const string PlayerLayerName = "Player";
        
        private int playerIndex;

        public PlayerStats GetPlayerStats() { return playerStats; }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        
        private void Awake()
        {
            won = false;

            spawnPosition = transform.position;

            farthestRow = spawnPosition.y;

            menuJoin = playerInput.actions[StringInputActionMenuJoin];
        }

        public void Initialize(GameManager sceneGameManager, ScoreManager sceneScoreManager, PauseManager scenePauseManager)
        {
            gameManager = sceneGameManager;
            scoreManager = sceneScoreManager;
            pauseManager = scenePauseManager;

            playerIndex = gameManager.GetPlayerPosition(this) + 1;
            
            Debug.Log("Controller: Player Index: " + playerIndex);
            
            //playerAnimator.Init(playerIndex);
        }
        
        private void Update()
        {
            ChoseMoveDirection();
        }

        private void ChoseMoveDirection()
        {
            Debug.Log("Movement Input: " + movementInput);
            
            switch (movementInput.y)
            {
                case > 0 when !leaping && keyReleased && !won:
                    keyReleased = false;
                    Rotate(Quaternion.Euler(-90f, 0f, 0f));
                    Move(Vector3.up);
                    
                    break;
                case < 0 when !leaping && keyReleased && !won:
                    keyReleased = false;
                    Rotate(Quaternion.Euler(90f, 0f, 180f));
                    Move(Vector3.down);
                    
                    break;
                default:
                {
                    switch (movementInput.x)
                    {
                        case > 0 when !leaping && keyReleased && !won:
                            keyReleased = false;
                            
                            Rotate(Quaternion.Euler(0f, 90f, -90f));
                            Move(Vector3.right);
                            
                            break;
                        case < 0 when !leaping && keyReleased && !won:
                            keyReleased = false;
                            
                            Rotate(Quaternion.Euler(0f, -90f, 90f));
                            Move(Vector3.left);
                            
                            break;
                        default:
                        {
                            if(movementInput is { y: 0, x: 0 } && !leaping)
                            {
                                keyReleased = true;
                            }

                            break;
                        }
                    }

                    break;
                }
            }
        }
        
        private void Move(Vector3 direction)
        {
            Debug.Log("Move: " + direction);
            
            var destination = transform.position + direction;

            var barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, 
                                                                        LayerMask.GetMask(BarrierLayerName));
            var platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, 
                                                                        LayerMask.GetMask(PlatformLayerName));
            var obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, 
                                                                        LayerMask.GetMask(ObstacleLayerName));
            var playerTwo = Physics2D.OverlapBox(destination, Vector2.zero, 0f, 
                                                                        LayerMask.GetMask(PlayerLayerName));
            
            if (barrier != null || playerTwo != null)
            {
                return;
            }
            
            transform.SetParent(platform != null ? platform.transform : null);
            
            if (obstacle != null && platform == null)
            {
                transform.position = destination;
                
                Death();
            } else
            {
                if (destination.y > farthestRow)
                {
                    farthestRow = destination.y;
                    
                    scoreManager.GivePointsForAdvancing(this);
                }
                
                StartCoroutine(Leap(destination));
            }
        }
        
        public void ClearParents()
        {
            transform.SetParent(null);
        }
        
        private IEnumerator Leap(Vector3 destination)
        {
            leaping = true;
            var startPosition = transform.position;
            var elapsed = 0f;
            
            playerAnimator.SetLeapingSprite();
            
            while (elapsed < maxLeapDuration)
            {
                var interpolationValue = elapsed / maxLeapDuration;
                transform.position = Vector3.Lerp(startPosition, destination, interpolationValue);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = destination;
            
            playerAnimator.SetIdleSprite();
            
            leaping = false;
        }
        
        private void Rotate(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
        
        public void Death()
        {
            StopAllCoroutines();

            //transform.rotation = Quaternion.identity;
            
            playerAnimator.SetDeadSprite();

            enabled = false;

            gameManager.Died();
        }
        
        public void SetWon(bool newWonState)
        {
            won = newWonState;
        }
        
        public bool GetWon()
        {
            return won;
        }
        
        public void Respawn()
        {
            StopAllCoroutines();
            
            leaping = false;
            Rotate(Quaternion.Euler(-90f, 0f, 0f));
            transform.position = spawnPosition;
            farthestRow = spawnPosition.y;
            
            playerAnimator.SetIdleSprite();
            gameObject.SetActive(true);
            
            won = false;
            enabled = true;
            
            playerAnimator.ResetAnimator();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
            {
                Death();
            }
        }
        
        public void ContinueGame()
        {
            if (menuJoin.WasPressedThisFrame() && pauseManager.IsGamePaused)
            {
                gameManager.ContinueGame();
            }
        }

        public void HandlePause(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            
            Pause?.Invoke();
        }

        public void HandleResume(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            
            Resume?.Invoke();
        }

        public void UseGameControls()
        {
            playerInput.SwitchCurrentActionMap(gameActionMapName);
        }

        public void UseMenuControls()
        {
            playerInput.SwitchCurrentActionMap(uiActionMapName);
        }
    }
}
