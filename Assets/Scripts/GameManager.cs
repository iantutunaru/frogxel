using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles the game logic
/// </summary>
public class GameManager : MonoBehaviour
{
    // Array of Homes that players can capture
    private Home[] homes;
    // Instance of the Lane Controller
    private LaneController laneController;
    // int used to keep track of time during rounds
    private int time;
    // GameObject containing title, title background and title text
    public GameObject startMenu;
    // GameObject containing game over text
    public GameObject gameOverMenu;
    // Text object that is used to dipslay victor's number and score
    public Text gameWonText;
    // Text object used with game overs and round won scenarios
    public Text playAgainText;
    // Text representation of the first player score
    public Text scoreTextPlayerOne;
    // Text reprentation of the second player score
    public Text scoreTextPlayerTwo;
    // Text represenation of the third player score
    public Text scoreTextPlayerThree;
    // Text representation of the fourth player score
    public Text scoreTextPlayerFour;
    // Array of scores of all players in the game
    private Text[] scores;
    // Text used to present time left to the players
    public Text timeText;
    // List of all players currenty playing
    private List<PlayerController> froggers = new List<PlayerController>();
    // Bool showing whenether the round is in progress or it has ended
    private bool gameEnded = false;
    // Instance of the Player Manager
    private PlayerManager playerManager;

    /// <summary>
    /// Initialize homes, lane controller, player manager, scores and disable play again text
    /// </summary>
    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        laneController = FindObjectOfType<LaneController>();
        playerManager = FindObjectOfType<PlayerManager>();

        scores = new Text[4] 
        {
            scoreTextPlayerOne, scoreTextPlayerTwo, scoreTextPlayerThree, scoreTextPlayerFour,
        };

        // Disable score if they are enabled
        foreach (Text score in scores)
        {
            score.enabled = false;
        }

        playAgainText.enabled = false;
    }

    /// <summary>
    /// Initialize the welcome screen
    /// </summary>
    private void Start()
    {
        WelcomeScreen();
    }

    /// <summary>
    /// Make sure that the start screen is active
    /// </summary>
    private void WelcomeScreen()
    {
        startMenu.SetActive(true);
    }

    /// <summary>
    /// Unpause the game, clear the lanes, disable game over and round ended texts, reset score for each player, fill lanes and start a new level
    /// </summary>
    private void NewGame()
    {
        gameEnded = false;
        laneController.EndGame();
        gameOverMenu.SetActive(false);
        gameWonText.enabled = false;
        playAgainText.enabled = false;

        // If there are players in the game
        if (froggers.Count > 0)
        {
            // Reset score of each player
            foreach (PlayerController frogger in froggers)
            {
                EnableScore(frogger);
            }
        }

        laneController.FillLanes();

        NewLevel();
    }

    /// <summary>
    /// Start a new level by enabling homes, start a new timer and respawn every player
    /// </summary>
    private void NewLevel()
    {
        // Hide trophy of each home to re-enable capturing
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }

        StartCoroutine(Timer(120));

        // Check if there are player in the game
        if (froggers.Count > 0)
        {
            // Respawn every player
            foreach (PlayerController frogger in froggers)
            {
                Respawn();
            }
        }
    }

    /// <summary>
    /// Check if player is either dead, captured a home or finished round and respawn them
    /// </summary>
    private void Respawn()
    {
        // Go through each player in the game
        foreach (PlayerController frogger in froggers)
        {
            // Check if player is dead, captured a home or finished the round and respawn them
            if (!frogger.enabled || frogger.GetWon())
            {
                frogger.Respawn();
            }
        }
    }

    /// <summary>
    /// When player joins a game then add them to the list of players, enable their score. If they are the first player then start the game
    /// </summary>
    public void OnPlayerJoined(PlayerController frogger)
    {
        froggers.Add(frogger);
        EnableScore(frogger);

        // If player is first then start the game
        if (froggers.Count == 1)
        {
            startMenu.SetActive(false);
            NewGame();
        }
    }

    /// <summary>
    /// Check what is the player's number
    /// </summary>
    public int GetPlayerPosition(PlayerController frogger) 
    {
        return froggers.IndexOf(frogger);
    }

    /// <summary>
    /// IEnumerator that kills all players if the timer runs down to 0
    /// </summary>
    private IEnumerator Timer(int duration)
    {
        time = duration;
        timeText.text = time.ToString();

        // decrease timer by 1 each second and update the clock in game
        while (time > 0)
        {
            yield return new WaitForSeconds(1);

            time--;
            timeText.text = time.ToString();
        }

        // Kill every player
        foreach (PlayerController frogger in froggers)
        {
            frogger.Death();
        }
    }

    /// <summary>
    /// Enable score of the selected player and set the score to 0
    /// </summary>
    private void EnableScore(PlayerController frogger)
    {
        scores[froggers.IndexOf(frogger)].enabled = true;
        SetScore(frogger, 0);
    }

    /// <summary>
    /// Set the score of the specified player and update the text in game
    /// </summary>
    private void SetScore(PlayerController frogger, int score)
    {
        frogger.SetScore(score);

        scores[froggers.IndexOf(frogger)].text = score.ToString();
    }

    /// <summary>
    /// Either respawn players if there is still time or end the round
    /// </summary>
    public void Died()
    {
        // If timer is not 0 then respawn the player, otherwise end the game
        if (time > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }

    /// <summary>
    /// Disable controls for each player, enable game over text and play again text, inform the player manager that the game is paused to disable controls and wait for one of the players
    /// to continue the game
    /// </summary>
    private void GameOver()
    {
        DisablePlayerControls();

        gameOverMenu.SetActive(true);
        playAgainText.enabled = true;
        gameEnded = true;
        playerManager.PauseGame();

        StopAllCoroutines();
        StartCoroutine(PlayAgain());
    }

    /// <summary>
    /// Wait for the game to continue
    /// </summary>
    private IEnumerator PlayAgain()
    {
        // If the game has not yet continued then wait
        while (gameEnded)
        {
            yield return null;
        }

        // Remove parents of each frogger so the game can respawn them properly
        foreach (PlayerController frogger in froggers)
        {
            frogger.ClearParents();
        }

        NewGame();
    }

    /// <summary>
    /// Continue the game and inform the player manager
    /// </summary>
    public void ContinueGame()
    {
        gameEnded = false;
        playerManager.UnpauseGame();
    }

    /// <summary>
    /// Method to check if the game has paused or ended by returning current boolean value
    /// </summary>
    public bool GameEnded()
    {
        return gameEnded;
    }

    /// <summary>
    /// Disable player controls, set and display number of the highest scoring player and their score, and wait for the game to restart
    /// </summary>
    private void GameWon()
    {
        DisablePlayerControls();

        gameWonText.text = $"PLAYER {GetHighestScorePlayerPosition() + 1} WON! SCORE: {froggers[GetHighestScorePlayerPosition()].GetScore()}";
        gameWonText.enabled = true;
        playAgainText.enabled = true;
        gameEnded = true;

        StopAllCoroutines();
        StartCoroutine(PlayAgain());
    }

    /// <summary>
    /// Disables player controls by setting their boolean to a won state
    /// </summary>
    private void DisablePlayerControls()
    {
        foreach (PlayerController frogger in froggers)
        {
            frogger.SetWon(true);
        }
    }

    /// <summary>
    /// Goes through the score of every player to find who has the highest one
    /// </summary>
    private int GetHighestScorePlayerPosition()
    {
        int highestScore = 0;
        int position = 0;

        // Go through each player in the game
        for (int i = 0; i < froggers.Count; i++)
        {
            // Find the highest score by comparing each score and store the number of the player with the highest score
            if (froggers[i].GetScore() > highestScore)
            {
                highestScore = froggers[i].GetScore();
                position = i;
            }
        }

        return position;
    }

    /// <summary>
    /// Reward player with 10 points for advancing to a new farthest row
    /// </summary>
    public void AdvancedRow(PlayerController frogger)
    {
        SetScore(frogger, frogger.GetScore() + 10);
    }

    /// <summary>
    /// When home is captured by the player then disable controls of that player, increase their score by a set amount and check if all homes have been captured.
    /// If all homes have been captured and this was the last home then give player a double reward and pause the game. Otherwise, respawn the player.
    /// </summary>
    public void HomeOccupied(PlayerController frogger)
    {
        frogger.SetWon(true);

        SetScore(frogger, frogger.GetScore() + 500);

        // Check if all homes have been capured
        if (Cleared())
        {
            SetScore(frogger, frogger.GetScore() + 1000);
            StopAllCoroutines();
            Invoke(nameof(GameWon), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    /// <summary>
    /// Go though each home and check if it's occupied
    /// </summary>
    private bool Cleared()
    {
        // Go through each home
        for (int i = 0; i < homes.Length;i++)
        {
            // If home is not occupied then return false
            if (!homes[i].enabled)
            {
                return false;
            }
        }

        return true;
    }
}
