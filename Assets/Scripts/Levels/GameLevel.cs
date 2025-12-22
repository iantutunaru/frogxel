
using Frogxel.Lanes;
using Managers;
using Player;
using UnityEngine;

namespace Frogxel.Levels
{
    public class GameLevel : Level
    {
        [SerializeField] private LanesController lanesController;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private PlayerManager playerManager;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            PlayerController.Pause += HandlePause;
            PlayerController.Resume += HandleResume;

            StartGame();
        }

        public override void OnExit()
        {
            PlayerController.Pause -= HandlePause;
            PlayerController.Resume -= HandleResume;
            
            base.OnExit();
        }

        private void StartGame()
        {
            lanesController.Init();
            pauseManager.UnpauseGame();
        }
        
        private void HandlePause()
        {
            playerManager.UseMenuControls();
            pauseManager.PauseGame();
        }
        
        private void HandleResume()
        {
            playerManager.UseGameControls();
            pauseManager.UnpauseGame();
        }
    }
}