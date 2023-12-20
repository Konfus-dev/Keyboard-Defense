using System;
using UnityEditor;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    [Serializable]
    public struct Word : IEquatable<Word>
    {
        public Word(string word, PlayerSettings.Switch.Languages wordLanguage, PromptDifficulty promptDifficulty, WordCommonality wordCommonality)
        {
            value = word;
            language = wordLanguage;
            difficulty = promptDifficulty;
            commonality = wordCommonality;
        }

        [SerializeField]
        private string value;
        public string Value => value;

        [SerializeField] 
        private PlayerSettings.Switch.Languages language;
        public PlayerSettings.Switch.Languages Language => language;

        [SerializeField] 
        private WordCommonality commonality;
        public WordCommonality Commonality => commonality; 
        
        [SerializeField] 
        private PromptDifficulty difficulty;
        public PromptDifficulty Difficulty => difficulty; 
        
        public override string ToString()
        {
            return value;
        }

        public bool Equals(Word other)
        {
            return value == other.value && difficulty == other.difficulty;
        }

        public override bool Equals(object obj)
        {
            return obj is Word other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, difficulty);
        }
        
        public static implicit operator string(Word word)
        {
            return word.value;
        }

        public static readonly Word None = default;
    }
}