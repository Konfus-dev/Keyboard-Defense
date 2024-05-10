using UnityEngine;

namespace KeyboardDefense.Scenes
{
    [CreateAssetMenu(menuName = "Keyboard Defense/New Scene Info", fileName = "New Scene Info")]
    public class SceneInfo : ScriptableObject
    {
        [SerializeField]
        private SceneType sceneType;
        public SceneType SceneType => sceneType;
        
        [SerializeField]
        private string sceneName;
        public string SceneName => sceneName;
        
        [SerializeField]
        private string sceneDescription;
        public string SceneDescription => sceneDescription;
        
        [SerializeField, Range(0, 5)]
        private int sceneStarDifficulty;
        public int SceneStarDifficulty => sceneStarDifficulty;

        [SerializeField] 
        private GameObject sceneMiniature;
        public GameObject SceneMiniature => sceneMiniature;

        [SerializeField] 
        private SceneInfo[] sceneDependencies;
        public SceneInfo[] SceneDependencies => sceneDependencies;
    }

    public enum SceneType
    {
        Menu,
        Level,
        QuitGame
    }
}