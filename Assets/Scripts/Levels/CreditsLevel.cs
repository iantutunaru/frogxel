using Frogxel.Animation;
using Frogxel.Managers;
using UnityEngine;

namespace Frogxel.Levels
{
    public class CreditsLevel : Level
    {
        [SerializeField] private CreditsAnimationEvents animationEvents;
        [SerializeField] private string mainMenuSceneName;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            animationEvents.CreditsEnd += HandleCreditsEnd;
        }

        public override void OnExit()
        {
            animationEvents.CreditsEnd -= HandleCreditsEnd;
            
            base.OnExit();
        }
        
        private void HandleCreditsEnd()
        {
            LevelManager.TryChangeLevel(mainMenuSceneName);
        }
    }
}