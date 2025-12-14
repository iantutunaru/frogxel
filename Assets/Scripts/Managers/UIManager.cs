using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // GameObject containing title, title background and title text
        [SerializeField] private GameObject startMenu;
    
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private ScoreManager scoreManager;
    
        // Array of scores of all players in the game
        [SerializeField] private Text[] scores;
        [SerializeField] private Text playAgainText;
        // Text object that is used to display the victor's number and score
        [SerializeField] private Text gameWonText;
        [SerializeField] private GameObject gameOverMenu;
        
        public void NewGame()
        {
            gameWonText.gameObject.SetActive(false);
            playAgainText.gameObject.SetActive(false);
            startMenu.SetActive(false);
            gameOverMenu.SetActive(false);
        }

        public void EnablePlayerScore(PlayerController player)
        {
            scores[playerManager.GetPlayers().IndexOf(player)].gameObject.SetActive(true);
        }

        public void SetPlayerScore(PlayerController player, int score)
        {
            scores[playerManager.GetPlayers().IndexOf(player)].text = score.ToString();
        }

        public void DisplayHighestScore()
        {
            gameWonText.text = $"PLAYER {scoreManager.GetHighestScorePlayerPosition() + 1} WON! " + 
                $"SCORE: {playerManager.GetPlayers()[scoreManager.GetHighestScorePlayerPosition()].GetPlayerStats().GetScore()}";
            
            gameWonText.gameObject.SetActive(true);
            
            DisplayPlayAgainText();
        }

        public void DisplayGameOver()
        {
            gameOverMenu.SetActive(true);
            
            DisplayPlayAgainText();
        }

        private void DisplayPlayAgainText()
        {
            playAgainText.gameObject.SetActive(true);
        }
    }
}
