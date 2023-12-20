using System;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    [Serializable]
    public struct Prompt : IEquatable<Prompt>
    {
        public Prompt(string prompt, PromptDifficulty promptDifficulty)
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
        
        public bool Equals(Prompt other)
        {
            return value == other.value && difficulty == other.difficulty;
        }

        public override bool Equals(object obj)
        {
            return obj is Prompt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, (int)difficulty);
        } 
        
        public static implicit operator string(Prompt prompt)
        {
            return prompt.value;
        }
        
        public static readonly Prompt None = default;
    }
}