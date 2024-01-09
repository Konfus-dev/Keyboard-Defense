using System;
using UnityEditor;
using UnityEngine;

namespace KeyboardCats.Data
{
    [Serializable]
    public struct WordData : IEquatable<WordData>
    {
        public WordData(string word, PlayerSettings.Switch.Languages wordLanguage, PromptDifficulty difficulty)
        {
            value = word;
            language = wordLanguage;
            this.difficulty = difficulty;
        }

        [SerializeField]
        private string value;
        public string Value => value;

        [SerializeField] 
        private PlayerSettings.Switch.Languages language;
        public PlayerSettings.Switch.Languages Language => language;
        
        [SerializeField] 
        private PromptDifficulty difficulty;
        public PromptDifficulty Difficulty => difficulty; 
        
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
        
        public override string ToString()
        {
            return value;
        }
        
        public static implicit operator string(WordData wordData)
        {
            return wordData.ToString();
        }
        
        public static implicit operator PromptData(WordData wordData)
        {
            return new PromptData(new [] { wordData }, wordData.difficulty);
        }

        public static readonly WordData None = default;
    }
}