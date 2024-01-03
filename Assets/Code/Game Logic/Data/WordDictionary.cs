using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Konfus.Utility.Extensions;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace KeyboardCats.Data
{
    public static class WordDictionary
    {
        private static readonly Dictionary<string, WordDictionaryEntry> _entries = new();
        private static readonly HttpClient _client = new();

        public static WordDictionaryEntry Lookup(string word)
        {
            // We have the def cached! Return from cache
            if (_entries.TryGetValue(word, out WordDictionaryEntry def)) return def;
            
            // Not in cache, look it up via the merriam webster dictionary api...
            var response = _client.GetAsync(
                $"https://www.dictionaryapi.com/api/v3/references/collegiate/json/{word}?key=7b992530-80f0-4dd7-ad57-d082018b6658").Result;
            
            // Couldn't look up for some reason or another
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"Failed to get definition from dictionary api: {response.StatusCode}");
                return WordDictionaryEntry.None;
            }
            
            // Read response as string, then deserialize it as json
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var dictInfo = JsonConvert.DeserializeObject<JArray>(responseBody);
                
            // We didn't find a definition
            if (dictInfo.IsNullOrEmpty()) return WordDictionaryEntry.None;
                
            // Try to get definition
            var firstDef = dictInfo.First();
            if (!firstDef.HasValues) return WordDictionaryEntry.None;
            // TODO: some valid defs don't have a shortdef, figure out how to identify them and how to get their definition!
            var shortDefs = firstDef["shortdef"]?.Values<string>().ToArray();
            
            // 3rd strike we are out...
            if (shortDefs.IsNullOrEmpty()) return WordDictionaryEntry.None;
                
            // Create definition struct and add to our defs
            var wordDef = new WordDictionaryEntry();
            _entries[word] = wordDef;
                
            // Return result
            return wordDef;

        }
        
        public static WordDictionaryEntry Lookup(WordData wordData)
        {
            return Lookup(wordData.ToString());
        }
    }
}