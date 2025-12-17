using Managers;
using Player;
using UnityEngine;

public class ScorePickup : PickupBase
{
    [SerializeField] private int points = 50;

    protected override void OnCollected(PlayerController player)
    {
        var scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(player, points);
            Debug.Log("Player get " + points + "points");
        }
    }
}
