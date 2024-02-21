using System;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class PromptManager : Singleton<PromptManager>
    {
        [Header("Word Databases")]
        [SerializeField]
        private WordDatabase easyWords;
        [SerializeField]
        private WordDatabase intermediateWords;
        [SerializeField]
        private WordDatabase hardWords;
        [SerializeField]
        private WordDatabase veryHardWords;
        
        public string GeneratePrompt(PromptDifficulty difficulty)
        {
            // TODO: intelligently generate prompts from words, as well as use random prompts and words from databases
            switch (difficulty)
            {
                case PromptDifficulty.Easy:
                    return GeneratePrompt(/*easyPrompts,*/ easyWords);
                case PromptDifficulty.Medium:
                    return GeneratePrompt(/*intermediatePrompts,*/ intermediateWords);
                case PromptDifficulty.Hard:
                    return GeneratePrompt(/*hardPrompts,*/ hardWords);
                case PromptDifficulty.VeryHard:
                    return GeneratePrompt(/*veryHardPrompts,*/ veryHardWords);
                default:
                    throw new NotImplementedException($"No logic implemented to generate a prompt with {difficulty} difficulty");
            }
        }
        
        private string GeneratePrompt(WordDatabase wordDatabase)
        {
            string prompt = wordDatabase.GetRandom();
            return prompt;
        }
    }
}