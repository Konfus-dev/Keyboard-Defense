﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Konfus.Utility.Extensions;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace KeyboardCats.Data
{
    public static class WordDictionary
    {
        //private static readonly Dictionary<string, WordDictionaryEntry> _entries = new();
        private static readonly HttpClient _client = new();
        
        public static async Task<WordDictionaryEntry> LookupAsync(WordData wordData)
        {
            // I'm not sure there is any use in caching the defs.. will just take up memory. We can wait for a quick interweb query :)
            // We have the def cached! Return from cache
            //if (_entries.TryGetValue(wordData, out WordDictionaryEntry def)) return def;
            
            // Not in cache, look it up via the merriam webster dictionary api...
            var response = await _client
                .GetAsync($"https://www.dictionaryapi.com/api/v3/references/collegiate/json/{wordData}?key=7b992530-80f0-4dd7-ad57-d082018b6658")
                .ContinueOnAnyContext();
            
            // Couldn't look up for some reason or another
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"Failed to get definition from dictionary api: {response.StatusCode}");
                return new WordDictionaryEntry(wordData, null);
            }
            
            // Read response as string, then deserialize it as json
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var dictInfo = JsonConvert.DeserializeObject<JArray>(responseBody);
                
            // We didn't find a definition
            if (dictInfo.IsNullOrEmpty()) return new WordDictionaryEntry(wordData, null);
                
            // Try to get definition
            var firstDef = dictInfo.First();
            if (!firstDef.HasValues) return new WordDictionaryEntry(wordData, null);;
            // TODO: some valid defs don't have a shortdef, figure out how to identify them and how to get their definition!
            var shortDefs = firstDef["shortdef"]?.Values<string>().ToArray();
            
            // We didn't find a def...
            if (shortDefs.IsNullOrEmpty()) return new WordDictionaryEntry(wordData, null);
                
            // Create definition struct and add to our defs
            var wordDef = new WordDictionaryEntry(wordData, shortDefs);
            //_entries[wordData] = wordDef;
                
            // Return result
            return wordDef;
        }
        
        public static WordDictionaryEntry Lookup(WordData wordData)
        {
            return LookupAsync(wordData).Result;
        }
    }
}