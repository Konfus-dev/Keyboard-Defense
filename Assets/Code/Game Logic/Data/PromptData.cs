using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeyboardCats.Data
{
    [Serializable]
    public struct PromptData : IEquatable<PromptData>
    {
        public PromptData(IEnumerable<WordData> promptWords, PromptDifficulty promptDifficulty)
        {
            words = promptWords.ToArray();
            value = string.Join(" ", words);
            difficulty = promptDifficulty;
        }
        
        [SerializeField]
        private WordData[] words;
        public WordData[] Words => words;
        
        
        [SerializeField]
        private string value;
        public string Value => value;
        
        [SerializeField]
        private PromptDifficulty difficulty;
        public PromptDifficulty Difficulty => difficulty;
        
        public bool Equals(PromptData other)
        {
            return value == other.value && difficulty == other.difficulty;
        }

        public override bool Equals(object obj)
        {
            return obj is PromptData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, (int)difficulty);
        } 
        
        public override string ToString()
        {
            return value;
        }

        public static implicit operator string(PromptData promptData)
        {
            return promptData.ToString();
        }
        
        public static readonly PromptData None = default;
    }
}