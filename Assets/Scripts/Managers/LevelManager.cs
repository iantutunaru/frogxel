using System;
using Frogxel.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Frogxel.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;
        
        private Level _currentLevel;
        private Awaitable _currentLevelChangeOperation;

        public static void TryChangeLevel(string levelName)
        {
            if (_instance == null)
            {
                return;
            }
            
            _instance.TryChangeLevelInternal(levelName);
        }
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                
                return;
            }

            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            SetCurrentLevel();
            
            _currentLevel.OnEnter();
        }
        
        private void SetCurrentLevel()
        {
            _currentLevel = FindFirstObjectByType(typeof(Level), FindObjectsInactive.Include) as Level;

            if (_currentLevel == null)
            {
                throw new Exception("No current level found!");
            }
        }
        
        private void TryChangeLevelInternal(string levelName)
        {
            if (_currentLevelChangeOperation != null)
            {
                return;
            }
            
            var loadSceneOperation = SceneManager.LoadSceneAsync(levelName);

            if (loadSceneOperation == null)
            {
                throw new Exception($"Could not load scene at path \"{levelName}\"");
            }
            
            _currentLevel.OnExit();

            _currentLevelChangeOperation = Awaitable.FromAsyncOperation(loadSceneOperation);
            
            _currentLevelChangeOperation.GetAwaiter().OnCompleted(HandleLevelLoaded);
        }

        private void HandleLevelLoaded()
        {
            _currentLevelChangeOperation = null;
            
            SetCurrentLevel();
            
            _currentLevel.OnEnter();
        }
    }
}