using System;
using KeyboardDefense.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    // TODO: May need to make into a singleton to persist between scenes so we can store and access current scene info
    public class SceneManager : GameService<ISceneManager>, ISceneManager
    {
        [SerializeField]
        private SceneTransitioner sceneTransitioner;
        
        // TODO: make use of the scene info scriptable obj!
        // this scene info will contain the name of the scene, the scene transition (music to play in next scene, transition effect), as well as and 
        // other info that we need to know about a scene. Perhaps it contains a ref to the scene itself? So it auto updates the name?
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
