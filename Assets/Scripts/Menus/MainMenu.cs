using Frogxel.Managers;
using UnityEngine;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string gameSceneName;
        [SerializeField] private string creditsSceneName;
        
        public void PlayGame()
        {
            LevelManager.TryChangeLevel(gameSceneName);
        }

        public void OpenCredits()
        {
            LevelManager.TryChangeLevel(creditsSceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}