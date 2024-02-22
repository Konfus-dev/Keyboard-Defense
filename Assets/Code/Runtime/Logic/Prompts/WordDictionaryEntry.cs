using System;
using System.Collections.Generic;
using System.Linq;
using Konfus.Utility.Extensions;
using UnityEngine;

namespace KeyboardDefense.Logic.Prompts
{
    [Serializable]
    public struct WordDictionaryEntry : IEquatable<WordDictionaryEntry>
    {
        public WordDictionaryEntry(string definedWord, IEnumerable<string> wordDefinitions, AudioClip wordPronunciation = null, IEnumerable<WordData> wordSynonyms = null)
        {
            word = definedWord;
            definitions = wordDefinitions?.ToArray();
            pronunciation = wordPronunciation;
            synonyms = wordSynonyms?.ToArray();
        }
        
        [SerializeField]
        private string word;
        public string Word => word;
        
        [SerializeField]
        private string[] definitions;
        private string[] Definition => definitions;
        
        [SerializeField]
        private AudioClip pronunciation;
        public AudioClip Pronunciation => pronunciation;
        
        [SerializeField]
        private WordData[] synonyms;
        public WordData[] Synonyms => synonyms;

        public override bool Equals(object obj)
        {
            return obj is WordDictionaryEntry other && Equals(other);
        }

        public bool Equals(WordDictionaryEntry other)
        {
            return word == other.word && Equals(definitions, other.definitions) && Equals(pronunciation, other.pronunciation) && Equals(synonyms, other.synonyms);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(word, definitions, pronunciation, synonyms);
        }

        public override string ToString()
        {
            return definitions.IsNullOrEmpty() 
                ? $"No definitions for {word}!"
                : $"{word}: {string.Join(", ", definitions)}";
        }
        
        public static implicit operator string(WordDictionaryEntry wordDef)
        {
            return wordDef.ToString();
        }
        
        public static readonly WordDictionaryEntry None = default;
    }
}