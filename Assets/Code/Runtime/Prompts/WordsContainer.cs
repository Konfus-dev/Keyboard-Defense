using System;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class WordsContainer : GameService<IWordDatabase>, IWordDatabase
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
        
        public PromptData GetRandomWordOfGivenDifficulty(WordDifficulty difficulty)
        {
            // TODO: intelligently generate prompts from words, as well as use random prompts and words from databases
            switch (difficulty)
            {
                case WordDifficulty.Easy:
                    return GetRandom(easyWords);
                case WordDifficulty.Medium:
                    return GetRandom(intermediateWords);
                case WordDifficulty.Hard:
                    return GetRandom(hardWords);
                case WordDifficulty.VeryHard:
                    return GetRandom(veryHardWords);
                default:
                    throw new NotImplementedException($"No logic implemented to generate a prompt with {difficulty} difficulty");
            }
        }
        
        private PromptData GetRandom(WordDatabase wordDatabase)
        {
            WordData wordData = wordDatabase.GetRandom();
            return new PromptData(wordData.Word, wordData.Definition, wordData.Locale);
        }
    }
}