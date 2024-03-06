namespace KeyboardDefense.Scenes
{
    public interface ISceneManager
    {
        void LoadScene(string sceneName);
        void LoadNextScene();
        void ReloadCurrentScene();
        void QuitGame();
    }
}