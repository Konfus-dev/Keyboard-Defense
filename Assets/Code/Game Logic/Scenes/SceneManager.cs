using System;
using Konfus.Utility.Design_Patterns;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.Scenes
{
    public class SceneManager : Singleton<SceneManager>
    {
        [SerializeField]
        private SceneTransitionManager sceneTransitionManager;
        
        public void LoadScene(string sceneName)
        {
            sceneTransitionManager.TransitionTo(sceneName);
        }

        public void LoadNextScene()
        {
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(nextSceneIndex).name);
        }

        public void ReloadCurrentScene()
        {
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            sceneTransitionManager.TransitionToQuit();
        }
    }
    
    [Serializable]
    public class ChangeSceneEvent : UnityEvent<ChangeSceneEventArgs> { }

}
