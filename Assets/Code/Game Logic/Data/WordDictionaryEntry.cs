using System;
using System.Collections.Generic;
using System.Linq;
using Konfus.Utility.Extensions;
using UnityEngine;

namespace KeyboardCats.Data
{
    [Serializable]
    public struct WordDictionaryEntry : IEquatable<WordDictionaryEntry>
    {
        public WordDictionaryEntry(WordData wordData, IEnumerable<string> wordDefinitions, AudioClip wordPronunciation, IEnumerable<WordData> wordSynonyms)
        {
            this.wordData = wordData;
            definitions = wordDefinitions.ToArray();
            pronunciation = wordPronunciation;
            synonyms = wordSynonyms?.ToArray();
        }
        
        [SerializeField]
        private WordData wordData;
        public WordData WordData => wordData;
        
        [SerializeField]
        private string[] definitions;
        private string[] Definition => definitions;
        
        [SerializeField]
        private AudioClip pronunciation;
        public AudioClip Pronunciation => pronunciation;
        
        [SerializeField]
        private WordData[] synonyms;
        public WordData[] Synonyms => synonyms;

        public bool Equals(WordDictionaryEntry other)
        {
            return wordData.Equals(other.wordData) && Equals(synonyms, other.synonyms);
        }

        public override bool Equals(object obj)
        {
            return obj is WordDictionaryEntry other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(wordData, synonyms);
        }

        public override string ToString()
        {
            return definitions.IsNullOrEmpty() 
                ? "No definitions found!" 
                : $"{wordData}: {string.Join(", ", definitions)}";
        }
        
        public static implicit operator string(WordDictionaryEntry wordDef)
        {
            return wordDef.ToString();
        }
        
        public static readonly WordDictionaryEntry None = default;
    }
}