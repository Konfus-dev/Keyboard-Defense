using System;
using UnityEditor;
using UnityEngine;

namespace KeyboardCats.Data
{
    [Serializable]
    public struct WordData : IEquatable<WordData>
    {
        public WordData(string word, PlayerSettings.Switch.Languages wordLanguage, PromptDifficulty promptDifficulty, WordCommonality wordCommonality)
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

        public bool Equals(WordData other)
        {
            return value == other.value && difficulty == other.difficulty;
        }

        public override bool Equals(object obj)
        {
            return obj is WordData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, difficulty);
        }
        
        public static implicit operator string(WordData wordData)
        {
            return wordData.value;
        }

        public static readonly WordData None = default;
    }
}