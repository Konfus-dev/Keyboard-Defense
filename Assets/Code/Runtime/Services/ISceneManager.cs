namespace KeyboardDefense.Scenes
{
    public interface ISceneManager
    {
        void LoadScene(string sceneName);
        void ReloadCurrentScene();
        void QuitGame();
    }
}