using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // GameObject containing title, title background and title text
        public GameObject startMenu;
    
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private ScoreManager scoreManager;
    
        // Array of scores of all players in the game
        [SerializeField] private Text[] scores;
        [SerializeField] private Text playAgainText;
        // Text object that is used to display the victor's number and score
        [SerializeField] private Text gameWonText;
        [SerializeField] private GameObject gameOverMenu;

        private void Awake()
        {
            Init();
        }
        
        private void Init()
        {
            // Disable score if they are enabled
            foreach (var score in scores)
            {
                score.enabled = false;
            }
            
            gameWonText.enabled = false;
            playAgainText.enabled = false;
            
            gameOverMenu.SetActive(false);
            startMenu.SetActive(true);
        }
        
        public void TurnOffStartMenu()
        {
            startMenu.SetActive(false);
        }

        public void EnablePlayerScore(PlayerController player)
        {
            scores[playerManager.GetPlayers().IndexOf(player)].enabled = true;
        }

        public void SetPlayerScore(PlayerController player, int score)
        {
            scores[playerManager.GetPlayers().IndexOf(player)].text = score.ToString();
        }

        public void DisplayHighestScore()
        {
            gameWonText.text = $"PLAYER {scoreManager.GetHighestScorePlayerPosition() + 1} WON! " + 
                        $"SCORE: {playerManager.GetPlayers()[scoreManager.GetHighestScorePlayerPosition()].GetScore()}";
            
            gameWonText.enabled = true;
            
            DisplayPlayAgainText();
        }

        public void DisplayGameOver()
        {
            gameOverMenu.SetActive(true);
            
            DisplayPlayAgainText();
        }

        private void DisplayPlayAgainText()
        {
            playAgainText.enabled = true;
        }
    }
}
