using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager playerInputManager;
        
        [SerializeField] private bool gamePaused = false;
        
        public bool IsGamePaused => gamePaused;
    
        /// <summary>
        /// Enable joining when the game is not paused or ended
        /// </summary>
        public void UnpauseGame()
        {
            playerInputManager.EnableJoining();
            
            gamePaused = false;
        }

        /// <summary>
        /// Disable player joining if the game is paused or ended
        /// </summary>
        public void PauseGame()
        {
            playerInputManager.DisableJoining();
            
            gamePaused = true;
        }
    }
}
