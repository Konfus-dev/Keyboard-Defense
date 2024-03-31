using UnityEngine;

namespace KeyboardDefense.Scenes
{
    [CreateAssetMenu(menuName = "Keyboard Defense/New Scene Info", fileName = "New Scene Info")]
    public class SceneInfo : ScriptableObject
    {
        [SerializeField]
        private string sceneName;
        public string SceneName => sceneName;
        
        [SerializeField]
        private string sceneDescription;
        public string SceneDescription => sceneDescription;
        
        [SerializeField]
        private int sceneStarDifficulty;
        public int SceneStarDifficulty => sceneStarDifficulty;
    }
}