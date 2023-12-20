using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    [Serializable]
    public struct WordDictionaryEntry : IEquatable<WordDictionaryEntry>
    {
        public WordDictionaryEntry(Word word, IEnumerable<string> wordDefinitions, AudioClip wordPronunciation, IEnumerable<Word> wordSynonyms)
        {
            this.word = word;
            definitions = wordDefinitions.ToArray();
            pronunciation = wordPronunciation;
            synonyms = wordSynonyms.ToArray();
        }
        
        [SerializeField]
        private Word word;
        public Word Word => word;
        
        [SerializeField]
        private string[] definitions;
        private string[] Definition => definitions;
        
        [SerializeField]
        private AudioClip pronunciation;
        public AudioClip Pronunciation => pronunciation;
        
        [SerializeField]
        private Word[] synonyms;
        public Word[] Synonyms => synonyms;

        public bool Equals(WordDictionaryEntry other)
        {
            return word.Equals(other.word) && Equals(synonyms, other.synonyms);
        }

        public override bool Equals(object obj)
        {
            return obj is WordDictionaryEntry other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(word, synonyms);
        }

        public override string ToString()
        {
            return word;
        }
        
        public static implicit operator string(WordDictionaryEntry wordDef)
        {
            return wordDef.word;
        }
        
        public static readonly WordDictionaryEntry None = default;
    }
}