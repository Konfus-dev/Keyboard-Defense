using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Data
{
    [CreateAssetMenu(fileName = "PromptDatabase", menuName = "Keyboard Cats/PromptDatabase", order = 1)]
    public class PromptDatabase : ScriptableObject
    {
        [SerializeField]
        private PromptData[] prompts;
        
        public PromptData GetRandom()
        {
            var randPromptIndex = Random.Range(0, prompts.Length - 1);
            return prompts[randPromptIndex];
        }
        
        public PromptData GetRandom(PromptDifficulty difficulty)
        {
            var randPromptIndex = Random.Range(0, prompts.Length - 1);
            return prompts[randPromptIndex];
        }
    }
}