using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Data
{
    // TODO: create editor script for this similar to our word database editor script...
    // we want the words to automatically be parsed from our prompts when we type them.
    // we also want a definition database, that stores a words definition
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