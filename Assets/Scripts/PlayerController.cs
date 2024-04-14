using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 2D player controller that handles user input, movement, animations and death/respawn mechanics
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Handles the player's visual representation in the game
    private SpriteRenderer spriteRenderer;
    // The position where the player character respawns
    private Vector3 spawnPosition;
    // Tracks the furthest row the player has reached
    private float farthestRow;
    // Indicates if the player is currently leaping
    private bool leaping = false;
    // Tracks if a key was released, used for input handling
    private bool keyReleased = true;
    // The default sprite when the player is idle
    public Sprite idleSprite;
    // Additional sprites for each player
    // Idle sprite for player one
    public Sprite idleSpriteOne;
    // Idle sprite for player two
    public Sprite idleSpriteTwo;
    // Idle sprite for player three
    public Sprite idleSpriteThree;
    // Idle sprite for player four
    public Sprite idleSpriteFour;
    // Array to contain all idle sprites of players currently in the game
    public Sprite[] idleSprites;
    // The default sprite when the player is moving
    public Sprite leapSprite;
    // Additional sprites for each player
    // Moving sprite for player one
    public Sprite leapSpriteOne;
    // Moving sprite for player two
    public Sprite leapSpriteTwo;
    // Moving sprite for player three
    public Sprite leapSpriteThree;
    // Moving sprite for player four
    public Sprite leapSpriteFour;
    // Array to contain all moving sprites of players currently in the game
    public Sprite[] leapSprites;
    // The default sprite when the player is dead
    public Sprite deadSprite;
    // Additional sprites for each player
    // Death sprite for player one
    public Sprite deadSpriteOne;
    // Death sprite for player two
    public Sprite deadSpriteTwo;
    // Death sprite for player three
    public Sprite deadSpriteThree;
    // Death sprite for player four
    public Sprite deadSpriteFour;
    // Array to contain all death sprites of players currently in the game
    public Sprite[] deadSprites;
    // Create instance of the GameManager
    private GameManager gameManager;
    // Initialize movement vector
    private Vector2 movementInput = Vector2.zero;
    // Initialize score variable
    private int score;
    // Bool representing if the player has just captured a home or the game has ended
    private bool won;
    // Istance of the Player Input
    PlayerInput playerInput;
    // Action to continue the game after it has ended
    InputAction menuJoin;

    /// <summary>
    /// Reads player's movement input from the new Unity Input System
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Sets instances to the game systems on player creation/connection and sets player visuals depending on the order
    /// </summary>
    private void Awake()
    {
        won = false;

        gameManager = FindObjectOfType<GameManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        SetSprites();

        spawnPosition = transform.position;

        farthestRow = spawnPosition.y;

        playerInput = GetComponent<PlayerInput>();

        menuJoin = playerInput.actions["MenuJoin"];
    }

    /// <summary>
    /// Handles player movement
    /// </summary>
    private void Update()
    {
        // checks what direction player wants to move and moves them exactly 1 unit in that direction and only if they are not moving already
        if (movementInput.y > 0 && !leaping && keyReleased && !won)
        {
            keyReleased = false;
            Move(Vector3.up);
        }
        else if (movementInput.y < 0 && !leaping && keyReleased && !won)
        {
            keyReleased = false;
            Move(Vector3.down);
        }
        else if (movementInput.x > 0 && !leaping && keyReleased && !won)
        {
            keyReleased = false;
            Rotate(Quaternion.Euler(0f, 0f, 0f));
            Move(Vector3.right);
        }
        else if (movementInput.x < 0 && !leaping && keyReleased && !won)
        {
            keyReleased = false;
            Rotate(Quaternion.Euler(0f, -180f, 0f));
            Move(Vector3.left);
        } else if(movementInput.y == 0 && movementInput.x == 0 && !leaping)
        {
            keyReleased = true;
        }
    }

    /// <summary>
    /// Sets player sprites for idle, leap and death states depending on the player order
    /// </summary>
    private void SetSprites()
    {
        idleSprites = new Sprite[4]
        {
            idleSpriteOne, idleSpriteTwo, idleSpriteThree,idleSpriteFour,
        };

        leapSprites = new Sprite[4]
        {
            leapSpriteOne, leapSpriteTwo, leapSpriteThree, leapSpriteFour,
        };

        deadSprites = new Sprite[4]
        {
            deadSpriteOne, deadSpriteTwo, deadSpriteThree, deadSpriteFour,
        };

        spriteRenderer.sprite = idleSprites[gameManager.GetPlayerPosition(this)];
        idleSprite = idleSprites[gameManager.GetPlayerPosition(this)];
        leapSprite = leapSprites[gameManager.GetPlayerPosition(this)];
        deadSprite = deadSprites[gameManager.GetPlayerPosition(this)];
    }

    /// <summary>
    /// Set method to set score safely
    /// </summary>
    public void SetScore(int score)
    {
        this.score = score;
    }

    /// <summary>
    /// Get method to get score safely
    /// </summary>
    public int GetScore() 
    { 
        return score; 
    }

    /// <summary>
    /// Moves player in the desired direction after checking what is at the destination
    /// </summary>
    private void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));
        Collider2D playerTwo = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Player"));

        // check if there is either a barrier or a player at the destination
        // if true then don't move
        if (barrier != null || playerTwo != null)
        {
            return;
        }

        // check if there is a platform at the destination
        if (platform != null)
        {
            // if there is, then make player a child of the platform so they can move together
            transform.SetParent(platform.transform);
        } else
        {
            // if the player leaves the platform then disconnect them
            transform.SetParent(null);
        }

        // if player touches an obstacle while not on a platform then they will die
        if (obstacle != null && platform == null)
        {
            transform.position = destination;
            Death();
        } else
        {
            // if there is no obstacle and the row is the farthest the player has been then set the destination to the farthest row
            if (destination.y > farthestRow)
            {
                farthestRow = destination.y;
                FindObjectOfType<GameManager>().AdvancedRow(this);
            }

            // finally, perform the movement
            StartCoroutine(Leap(destination));
        }
    }

    /// <summary>
    /// Clear parents of the player. For example, used to disconnect player from the platform
    /// </summary>
    public void ClearParents()
    {
        transform.SetParent(null);
    }

    /// <summary>
    /// Moves player to the destination smoothly as to simulate the leaping motion
    /// </summary>
    private IEnumerator Leap(Vector3 destination)
    {
        leaping = true;
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        float duration = 0.125f;
        spriteRenderer.sprite = leapSprite;

        // move the player to the destination over a set period of time
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
        leaping = false;
    }

    /// <summary>
    /// Set method to set rotation safely
    /// </summary>
    private void Rotate(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    /// <summary>
    /// Stops all coroutines when player dies and informs the Game Manager
    /// </summary>
    public void Death()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        enabled = false;

        FindObjectOfType<GameManager>().Died();
    }

    /// <summary>
    /// Enable or disables the "won" state for the player
    /// </summary>
    public void SetWon(bool won)
    {
        this.won = won;
    }

    /// <summary>
    /// Get the current "won" state
    /// </summary>
    public bool GetWon()
    {
        return won;
    }

    /// <summary>
    /// Respawns the player and resets variables and sprites
    /// </summary>
    public void Respawn()
    {
        StopAllCoroutines();
        leaping = false;

        transform.rotation = Quaternion.identity;
        transform.position = spawnPosition;
        farthestRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        won = false;
        enabled = true;
    }

    /// <summary>
    /// Checks if player's collider box has inersected the collider box of the obstacle
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();
        }
    }

    /// <summary>
    /// Allows the player to continue the game if it has ended and the player pressed either "Space" or "Start"
    /// </summary>
    public void ContinueGame()
    {
        // Check if either "Space" or "Start" have been pressed and the game has eneded
        if (menuJoin.WasPressedThisFrame() && gameManager.GameEnded())
        {
            gameManager.ContinueGame();
        }
    }
}
