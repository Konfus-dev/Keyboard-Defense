using KeyboardDefense.Scenes;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KeyboardDefense.Services
{
    public interface ISceneManager : IGameService
    {
        public UnityEvent QuitGame { get; }
        UnityEvent ChangedScene { get; }
        
        SceneInfo CurrentScene { get; }
        
        public Scene[] GetOpenScenes();
        void LoadScene(SceneInfo sceneName);
        void ReloadCurrentScene();
        void Quit();
    }
}