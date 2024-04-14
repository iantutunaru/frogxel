using UnityEngine;

/// <summary>
/// Class that handles the behaviour of the home bases in their interactions with the player
/// </summary>
public class Home : MonoBehaviour
{
    // A trophy that appears when the base is claimed
    public GameObject trophy;

    /// <summary>
    /// Makes trophy visible
    /// </summary>
    private void OnEnable()
    {
        trophy.SetActive(true);
    }

    /// <summary>
    /// Makes trophy invisible
    /// </summary>
    private void OnDisable()
    {
        trophy.SetActive(false);
    }

    /// <summary>
    /// Handles interaction with a player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player touches the home base then inform Game Manager
        if (other.tag == "Player")
        {
            enabled = true;

            FindObjectOfType<GameManager>().HomeOccupied(other.GetComponent<PlayerController>());
        }
    }
}
