using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    public class PromptManager : Singleton<PromptManager>
    {
        [SerializeField]
        private PromptDatabase promptDatabase;
        [SerializeField]
        private WordDatabase wordDatabase;

        public string GeneratePrompt()
        {
            // TODO: intelligently generate prompts from words, as well as use random prompts from databases
            string prompt;
            var randChoice = Random.Range(0, 1);
            if (randChoice == 0) prompt = promptDatabase.GetRandom();
            else prompt = wordDatabase.GetRandom();
            return prompt;
        }
    }
}