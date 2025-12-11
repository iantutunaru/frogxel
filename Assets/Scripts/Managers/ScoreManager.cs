using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private int scoreForAdvancingRow = 10;
    
        /// <summary>
        /// Enable score of the selected player and set the score to 0
        /// </summary>
        public void EnableScore(PlayerController player)
        {
            SetScore(player, 0);
        
            uiManager.EnablePlayerScore(player);
        }
    
        /// <summary>
        /// Set the score of the specified player and update the text in game
        /// </summary>
        public void SetScore(PlayerController player, int score)
        {
            player.SetScore(score);
        
            uiManager.SetPlayerScore(player, score);
        }

        public void ResetPlayerScore()
        {
            foreach (var player in playerManager.GetPlayers())
            {
                EnableScore(player);
            }
        }
        
        /// <summary>
        /// Reward player with 10 points for advancing to a new farthest row
        /// </summary>
        public void GivePointsForAdvancing(PlayerController player)
        {
            SetScore(player, player.GetScore() + scoreForAdvancingRow);
        }
        
        /// <summary>
        /// Goes through the score of every player to find who has the highest one
        /// </summary>
        public int GetHighestScorePlayerPosition()
        {
            var highestScore = 0;
            var position = 0;

            // Go through each player in the game
            for (var i = 0; i < playerManager.GetPlayers().Count; i++)
            {
                // Find the highest score by comparing each score and store the number of the player with the highest score
                if (playerManager.GetPlayers()[i].GetScore() > highestScore)
                {
                    highestScore = playerManager.GetPlayers()[i].GetScore();
                    position = i;
                }
            }

            return position;
        }
    }
}
