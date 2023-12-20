using System.Collections.Generic;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    [CreateAssetMenu(fileName = "WordDatabase", menuName = "Keyboard Cats/WordDatabase", order = 1)]
    public class WordDatabase : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private List<Word> words;
        public List<Word> Words => words;
        
        private readonly Dictionary<string, Word> _wordDict = new();

        public Word GetRandom()
        {
            var randWordIndex = Random.Range(0, words.Count - 1);
            return words[randWordIndex];
        }
        
        public Word Get(string word)
        {
            Word cachedVal = _wordDict.TryGetValue(word, out var value) ? value : Word.None;
            return cachedVal;
        }

        public void Add(Word word)
        {
            if (_wordDict.TryAdd(word, word)) words.Add(word);
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
            foreach (Word word in words)
            {
                _wordDict.TryAdd(word, word);
            }
        }
    }
}