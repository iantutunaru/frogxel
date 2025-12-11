using System.Linq;
using UnityEngine;

namespace Managers
{
    public class CapturePointManager : MonoBehaviour
    {
        // Array of Homes that players can capture
        [SerializeField] private Home[] capturePoints;
        
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameManager gameManager;
        
        public Home[] GetCapturePoints() { return capturePoints; }
        
        /// <summary>
        /// When home is captured by the player then disable controls of that player, increase their score by a set amount and check if all homes have been captured.
        /// If all homes have been captured and this was the last home then give player a double reward and pause the game. Otherwise, respawn the player.
        /// </summary>
        public void GivePointsForCapturingPoint(PlayerController player)
        {
            player.SetWon(true);

            scoreManager.SetScore(player, player.GetScore() + 500);

            // Check if all homes have been captured
            if (Cleared())
            {
                scoreManager.SetScore(player, player.GetScore() + 1000);
                StopAllCoroutines();
                Invoke(nameof(AllBasesCaptured), 1f);
            }
            else
            {
                Invoke(nameof(RespawnPlayer), 1f);
            }
        }

        private void RespawnPlayer()
        {
            gameManager.Respawn();
        }

        private void AllBasesCaptured()
        {
            gameManager.AllBasesCaptured();
        }
        
        /// <summary>
        /// Go though each home and check if it's occupied
        /// </summary>
        private bool Cleared()
        {
            // Go through each home
            return capturePoints.All(home => home.enabled);
        }

        public void ResetCapturePoints()
        {
            // Hide trophy of each home to re-enable capturing
            foreach (var home in capturePoints)
            {
                home.enabled = false;
            }
        }
    }
}
