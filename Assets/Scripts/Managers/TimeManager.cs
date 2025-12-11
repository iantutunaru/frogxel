using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        // int used to keep track of time during rounds
        [SerializeField] int time;
        // Text used to present time left to the players
        [SerializeField] private Text timeText;
        
        [SerializeField] private PlayerManager playerManager;
        
        public int GetTime => time;
        
        /// <summary>
        /// IEnumerator that kills all players if the timer runs down to 0
        /// </summary>
        public IEnumerator Timer(int duration)
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
            foreach (var player in playerManager.GetPlayers())
            {
                player.Death();
            }
        }
    }
}
