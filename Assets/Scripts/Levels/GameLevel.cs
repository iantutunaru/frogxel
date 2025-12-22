
using Frogxel.Lanes;
using UnityEngine;

namespace Frogxel.Levels
{
    public class GameLevel : Level
    {
        [SerializeField] private LanesController lanesController;
        
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
        }
    }
}