using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeyboardDefense.Prompts
{
    [Serializable]
    public struct WordData : IEquatable<WordData>
    {
        public WordData(string word, string def, PlayerSettings.Switch.Languages wordLanguage, PromptDifficulty difficulty)
        {
            this.word = word;
            definition = def;
            language = wordLanguage;
            this.difficulty = difficulty;
        }

        [FormerlySerializedAs("value")] [SerializeField]
        private string word;
        public string Word => word;
        
        [SerializeField] 
        private string definition;
        public string Definition => definition; 

        [SerializeField] 
        private PlayerSettings.Switch.Languages language;
        public PlayerSettings.Switch.Languages Language => language;
        
        [SerializeField] 
        private PromptDifficulty difficulty;
        public PromptDifficulty Difficulty => difficulty; 
        
        public bool Equals(WordData other)
        {
            return word == other.word && difficulty == other.difficulty;
        }

        public override bool Equals(object obj)
        {
            return obj is WordData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(word, difficulty);
        }
        
        public override string ToString()
        {
            return word;
        }
        
        public static implicit operator string(WordData wordData)
        {
            return wordData.ToString();
        }

        public static readonly WordData None = default;
    }
}