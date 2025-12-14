using System.Collections;
using Frogxel.Lanes;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Class that handles the game logic
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LanesController lanesController;
        
        [Header("Managers")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private CapturePointManager capturePointManager;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private TimeManager timeManager;
        
        /// <summary>
        /// Unpause the game, clear the lanes, disable game over and round ended texts, reset score for each player, fill lanes and start a new level
        /// </summary>
        public void NewGame()
        {
            uiManager.NewGame();
            pauseManager.UnpauseGame();
            lanesController.Clear();
            
            // If there are players in the game
            if (playerManager.GetPlayers().Count > 0)
            {
                scoreManager.ResetPlayerScore();
            }

            lanesController.Init();

            NewLevel();
        }

        /// <summary>
        /// Start a new level by enabling homes, start a new timer and respawn every player
        /// </summary>
        private void NewLevel()
        {
            capturePointManager.ResetCapturePoints();

            StartCoroutine(timeManager.Timer(120));

            // Check if there are player in the game
            if (playerManager.GetPlayers().Count <= 0) return;
        
            Respawn();
        }

        /// <summary>
        /// Check if player is either dead, captured a home or finished round and respawn them
        /// </summary>
        public void Respawn()
        {
            // Go through each player in the game
            foreach (var player in playerManager.GetPlayers())
            {
                // Check if player is dead, captured a home or finished the round and respawn them
                if (!player.enabled || player.GetWon())
                {
                    player.Respawn();
                }
            }
        }
        
        /// <summary>
        /// Check what is the player's number
        /// </summary>
        public int GetPlayerPosition(PlayerController frogger) 
        {
            return playerManager.GetPlayers().IndexOf(frogger);
        }
        
        /// <summary>
        /// Either respawn players if there is still time or end the round
        /// </summary>
        public void Died()
        {
            // If timer is not 0 then respawn the player, otherwise end the game
            Invoke(timeManager.GetTime > 0 ? nameof(Respawn) : nameof(GameOver), 1f);
        }

        /// <summary>
        /// Wait for the game to continue
        /// </summary>
        private IEnumerator PlayAgain()
        {
            // If the game has not yet continued then wait
            while (pauseManager.IsGamePaused)
            {
                yield return null;
            }

            // Remove parents of each frogger so the game can respawn them properly
            foreach (var player in playerManager.GetPlayers())
            {
                player.ClearParents();
            }

            NewGame();
        }

        /// <summary>
        /// Continue the game and inform the player manager
        /// </summary>
        public void ContinueGame()
        {
            pauseManager.UnpauseGame();
        }

        
        /// <summary>
        /// Disable player controls, set and display number of the highest scoring player and their score, and wait for the game to restart
        /// </summary>
        public void AllBasesCaptured()
        {
            GameEnd();

            uiManager.DisplayHighestScore();

            RestartGame();
        }
        
        /// <summary>
        /// Disable controls for each player, enable game over text and play again text, inform the player manager that the game is paused to disable controls and wait for one of the players
        /// to continue the game
        /// </summary>
        public void GameOver()
        {
            GameEnd();

            uiManager.DisplayGameOver();

            RestartGame();
        }

        private void GameEnd()
        {
            playerManager.DisablePlayerControls();

            pauseManager.PauseGame();
        }

        private void RestartGame()
        {
            StopAllCoroutines();
            StartCoroutine(PlayAgain());
        }
    }
}
