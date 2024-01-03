using System;
using UnityEngine;

namespace KeyboardCats.Data
{
    [Serializable]
    public struct PromptData : IEquatable<PromptData>
    {
        public PromptData(string prompt, PromptDifficulty promptDifficulty)
        {
            value = prompt;
            difficulty = promptDifficulty;
        }
        
        [SerializeField]
        private string value;
        public string Value => value;
        
        [SerializeField]
        private PromptDifficulty difficulty;
        public PromptDifficulty Difficulty => difficulty;
        
        public override string ToString()
        {
            return value;
        }
        
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
        
        public static implicit operator string(PromptData promptData)
        {
            return promptData.value;
        }
        
        public static readonly PromptData None = default;
    }
}