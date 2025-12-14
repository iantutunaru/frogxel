using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        // Initialize score variable
        private int score;
        
        /// <summary>
        /// Set method to set score safely
        /// </summary>
        public void SetScore(int newScore)
        {
            this.score = newScore;
        }

        /// <summary>
        /// Get method to get score safely
        /// </summary>
        public int GetScore() 
        { 
            return score; 
        }
    }
}
