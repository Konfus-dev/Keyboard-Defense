using System.Collections.Generic;
using UnityEngine;

namespace KeyboardCats.Data
{
    [CreateAssetMenu(fileName = "WordDatabase", menuName = "Keyboard Cats/WordDatabase", order = 1)]
    public class WordDatabase : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private List<WordData> words;
        public List<WordData> Words => words;
        
        private readonly Dictionary<string, WordData> _wordDict = new();

        public WordData GetRandom()
        {
            var randWordIndex = Random.Range(0, words.Count - 1);
            return words[randWordIndex];
        }
        
        public WordData Get(string word)
        {
            WordData cachedVal = _wordDict.TryGetValue(word, out var value) ? value : WordData.None;
            return cachedVal;
        }

        public void Add(WordData wordData)
        {
            if (_wordDict.TryAdd(wordData, wordData)) words.Add(wordData);
        }

        internal void Clear()
        {
            words.Clear();
            _wordDict.Clear();
        }

        private void Awake()
        {
            RefreshCache();
        }

        private void RefreshCache()
        {
            foreach (WordData word in words)
            {
                _wordDict.TryAdd(word, word);
            }
        }
    }
}