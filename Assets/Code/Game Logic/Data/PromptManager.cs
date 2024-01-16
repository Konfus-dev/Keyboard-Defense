﻿using System;
using Konfus.Utility.Design_Patterns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Data
{
    public class PromptManager : Singleton<PromptManager>
    {
        // TODO: implement support for prompts! Mostly need UI/visual support for this..
        // To do this we need words to be separate concepts in UI.
        // Rn they can't be individually hovered over/clicked on and we can't do effects on a per word basis either...
        
        /*[Header("Prompt Databases")]
        [SerializeField]
        private PromptDatabase easyPrompts;
        [SerializeField]
        private PromptDatabase intermediatePrompts;
        [SerializeField]
        private PromptDatabase hardPrompts;
        [SerializeField]
        private PromptDatabase veryHardPrompts;*/
        [Header("Word Databases")]
        [SerializeField]
        private WordDatabase easyWords;
        [SerializeField]
        private WordDatabase intermediateWords;
        [SerializeField]
        private WordDatabase hardWords;
        [SerializeField]
        private WordDatabase veryHardWords;
        
        public PromptData GeneratePrompt(PromptDifficulty difficulty)
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
        
        private PromptData GeneratePrompt(/*PromptDatabase promptDatabase,*/ WordDatabase wordDatabase)
        {
            // TODO: intelligently generate prompts from words, as well as use random prompts and words from databases
            PromptData prompt;
            /*var randChoice = Random.Range(0, 2);
            if (randChoice == 0) prompt = promptDatabase.GetRandom();
            else*/ prompt = wordDatabase.GetRandom();
            return prompt;
        }
    }
}