using System;
using System.Globalization;
using KeyboardDefense.Localization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeyboardDefense.Prompts
{
    [Serializable]
    public struct WordData : IEquatable<WordData>
    {
        public WordData(string word, string def, Locale wordLocale, WordDifficulty difficulty)
        {
            this.word = word;
            definition = def;
            locale = wordLocale;
            this.difficulty = difficulty;
        }

        [SerializeField]
        private string word;
        public string Word => word;
        
        [SerializeField] 
        private string definition;
        public string Definition => definition; 

        [SerializeField]
        private Locale locale;
        public Locale Locale => locale;
        
        [SerializeField] 
        private WordDifficulty difficulty;
        public WordDifficulty Difficulty => difficulty; 
        
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