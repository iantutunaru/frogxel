using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class that handles players joining in the middle of the play session
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // Initialize Game Manager
    private GameManager gameManager;
    // Initialize Player
    private PlayerInputManager playerInputManager;

    /// <summary>
    /// Find player input manager
    /// </summary>
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    /// <summary>
    /// Enable joining when the game is not paused or ended
    /// </summary>
    public void UnpauseGame()
    {
        playerInputManager.EnableJoining();
    }

    /// <summary>
    /// Disable player joining if the game is paused or ended
    /// </summary>
    public void PauseGame()
    {
        playerInputManager.DisableJoining();
    }

    /// <summary>
    /// Call Game Manager and let them handle an addition of a new player
    /// </summary>
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        gameManager = FindObjectOfType<GameManager>();

        PlayerController frogger = playerInput.GetComponent<PlayerController>();

        gameManager.OnPlayerJoined(frogger);
    }
}
