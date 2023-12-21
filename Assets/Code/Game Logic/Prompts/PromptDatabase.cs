using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Prompts
{
    [CreateAssetMenu(fileName = "PromptDatabase", menuName = "Keyboard Cats/PromptDatabase", order = 1)]
    public class PromptDatabase : ScriptableObject
    {
        [SerializeField]
        private Prompt[] prompts;
        
        public Prompt GetRandom()
        {
            var randPromptIndex = Random.Range(0, prompts.Length - 1);
            return prompts[randPromptIndex];
        }
        
        public Prompt GetRandom(PromptDifficulty difficulty)
        {
            var randPromptIndex = Random.Range(0, prompts.Length - 1);
            return prompts[randPromptIndex];
        }
    }
}