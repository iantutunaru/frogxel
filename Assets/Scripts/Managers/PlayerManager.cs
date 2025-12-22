using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    /// <summary>
    /// Class that handles players joining in the middle of the play session
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PauseManager pauseManager;
        
        // List of all players currently playing
        private List<PlayerController> players = new List<PlayerController>();

        public List<PlayerController> GetPlayers()
        {
            return players;
        }

        /// <summary>
        /// Call Game Manager and let them handle an addition of a new player
        /// </summary>
        public void OnPlayerJoined(PlayerInput newPlayerInput)
        {
            var newPlayer = newPlayerInput.GetComponent<PlayerController>();
            
            newPlayer.Initialize(gameManager, scoreManager, pauseManager);
            
            players.Add(newPlayer);
            
            scoreManager.EnableScore(newPlayer);

            // If player is first then start the game
            if (players.Count != 1) return;
            
            gameManager.NewGame();
        }
        
        /// <summary>
        /// Disables player controls by setting their boolean to a won state
        /// </summary>
        public void DisablePlayerControls()
        {
            foreach (var player in players)
            {
                player.SetWon(true);
            }
        }

        public void UseGameControls()
        {
            foreach (var player in players)
            {
                player.UseGameControls();
            }
        }

        public void UseMenuControls()
        {
            foreach (var player in players)
            {
                player.UseMenuControls();
            }
        }
    }
}
