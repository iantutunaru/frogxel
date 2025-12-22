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
            HideCursor();
            ResumeTime();
            
            playerManager.UseGameControls();
            pauseManager.UnpauseGame();
            uiManager.ShowGameUi();
        }

        public void HandleQuit()
        {
            ShowCursor();
            ResumeTime();
            
            LevelManager.TryChangeLevel(mainMenuSceneName);
        }

        private void StartGame()
        {
            HideCursor();
            
            lanesController.Init();
            pauseManager.UnpauseGame();
        }
        
        private void HandlePause()
        {
            ShowCursor();
            PauseTime();
            
            playerManager.UseMenuControls();
            pauseManager.PauseGame();
            uiManager.ShowPauseMenu();
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void PauseTime()
        {
            Time.timeScale = 0;
        }

        private void ResumeTime()
        {
            Time.timeScale = 1;
        }
    }
}