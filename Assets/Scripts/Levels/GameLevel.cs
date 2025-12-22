using Frogxel.Lanes;
using Frogxel.Managers;
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
        [SerializeField] private UIManager uiManager;
        [SerializeField] private string mainMenuSceneName;
        
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
        
        public void HandleResume()
        {
            playerManager.UseGameControls();
            pauseManager.UnpauseGame();
            uiManager.ShowGameUi();
        }

        public void HandleQuit()
        {
            LevelManager.TryChangeLevel(mainMenuSceneName);
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
            uiManager.ShowPauseMenu();
        }
    }
}