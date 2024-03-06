using System;
using KeyboardDefense.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public class SceneManager : GameService<ISceneManager>, ISceneManager
    {
        [SerializeField]
        private SceneTransitioner sceneTransitioner;
        
        public void LoadScene(string sceneName)
        {
            sceneTransitioner.TransitionTo(sceneName);
        }

        // Doesn't work for some reason... don't need it so just commenting it out
        /*public void LoadNextScene()
        {
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex > UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            
            var nextScene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(nextSceneIndex);
            LoadScene(nextScene.name);
        }*/

        public void ReloadCurrentScene()
        {
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            sceneTransitioner.TransitionToQuit();
        }
    }
    
    [Serializable]
    public class ChangeSceneEvent : UnityEvent<ChangeSceneEventArgs> { }

}
