
using Frogxel.Lanes;
using Managers;
using UnityEngine;

namespace Frogxel.Levels
{
    public class GameLevel : Level
    {
        [SerializeField] private LanesController lanesController;
        [SerializeField] private PauseManager pauseManager;
        
        public override void OnEnter()
        {
            base.OnEnter();

            StartGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void StartGame()
        {
            lanesController.Init();
            pauseManager.UnpauseGame();
        }
    }
}