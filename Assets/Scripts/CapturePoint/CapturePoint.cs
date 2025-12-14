using System;
using Player;
using UnityEngine;

/// <summary>
/// Class that handles the behavior of the capture points in their interactions with the player
/// </summary>
public class CapturePoint : MonoBehaviour
{
    public static event Action<PlayerController> Captured;
    
    [SerializeField] private GameObject trophy;
    [SerializeField] private string playerTag;
    
    public bool IsCaptured { get; private set; }
    
    public void Clear()
    {
        IsCaptured = false;
        
        trophy.SetActive(false);
    }

    /// <summary>
    /// Handles interaction with a player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player touches the home base then inform Game Manager
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        var player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            throw new Exception($"\"{other.gameObject.name}\" has \"{playerTag}\" but is not a player and tried to capture a point \"{name}\"");
        }

        TryCapture(player);
    }

    private void TryCapture(PlayerController player)
    {
        if (IsCaptured)
        {
            return;
        }
        
        IsCaptured = true;
        
        trophy.SetActive(true);
        
        Captured?.Invoke(player);
    }
}
