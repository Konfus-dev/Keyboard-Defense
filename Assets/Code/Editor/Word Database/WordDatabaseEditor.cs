using System;
using System.Linq;
using KeyboardDefense.Localization;
using KeyboardDefense.Prompts;
using UnityEditor;
using UnityEngine;

namespace KeyboardDefense.Editor.Word_Database
{
    [CustomEditor(typeof(WordDatabase))]
    public class WordDatabaseEditor : UnityEditor.Editor
    {
        private int _currentPageIndex;
        private Vector2 _currentPageScrollAmount;
        
        //private Regex _nonAlphabetFilter;
        private WordDatabase _wordDatabase;
        private WordDifficulty _difficulty;
        private Locale _locale;

        private void Awake()
        {
            _wordDatabase = (WordDatabase)target;
            
            if (!_wordDatabase.Words.Any()) return;
            _difficulty = _wordDatabase.Words.FirstOrDefault().Difficulty;
            //_nonAlphabetFilter = new Regex("[^a-zA-Z]");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawWordPages();
            DrawImportSettings();
        }

        private void DrawWordPages()
        {
            const int countPerPage = 50;
            var wordDatabaseWordCount = _wordDatabase.Words.Any() ? _wordDatabase.Words.Count : 0;
            var numberPages = wordDatabaseWordCount / countPerPage;
            
            // Page header
            EditorGUILayout.LabelField("Words", EditorStyles.boldLabel);
            EditorGUIExtensions.Line(2);
            
            // Page number
            {
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("<", GUILayout.Width(25)))
                {
                    if (_currentPageIndex == 0) _currentPageIndex = 0;
                    else _currentPageIndex--;
                }

                EditorGUILayout.Space(50, expand: true);
                EditorGUILayout.LabelField("Page:", GUILayout.MaxWidth(35));
                _currentPageIndex = EditorGUILayout.IntField(_currentPageIndex, GUILayout.MaxWidth(55));
                EditorGUILayout.LabelField($"/{numberPages}", GUILayout.MaxWidth(55));
                EditorGUILayout.Space(50, expand: true);

                if (GUILayout.Button(">", GUILayout.Width(25)))
                {
                    if (_currentPageIndex == numberPages) _currentPageIndex = numberPages;
                    else _currentPageIndex++;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUIExtensions.Line();
            }
            
            // Page content
            {
                var pageBackgroundColor = EditorGUIExtensions.MakeBackgroundTexture(10, 10, new Color(0.25f, 0.25f, 0.25f));
                var pageStyle = new GUIStyle(EditorStyles.inspectorFullWidthMargins)
                {
                    normal = { background = pageBackgroundColor },
                    active = { background = pageBackgroundColor },
                    focused = { background = pageBackgroundColor }
                };
                
                var wordItemBackgroundColor = EditorGUIExtensions.MakeBackgroundTexture(10, 10, new Color(0.2f, 0.2f, 0.2f));
                var wordItemStyle = new GUIStyle(EditorStyles.inspectorFullWidthMargins)
                {
                    normal = { background = wordItemBackgroundColor },
                    active = { background = wordItemBackgroundColor },
                    focused = { background = wordItemBackgroundColor }
                };
                
                _currentPageScrollAmount = EditorGUILayout.BeginScrollView(_currentPageScrollAmount, pageStyle, GUILayout.Height(500));
                
                int startingIndex = _currentPageIndex * countPerPage;
                for (int wordIndex = startingIndex; wordIndex < Mathf.Min(wordDatabaseWordCount, startingIndex + countPerPage); wordIndex++)
                {
                    var wordData = _wordDatabase.Words[wordIndex];
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField($"[{wordIndex}] {wordData}:", EditorStyles.boldLabel);
                    
                    EditorGUIExtensions.Line();
                    EditorGUILayout.BeginVertical(wordItemStyle);
                    
                    var newWord = EditorGUILayout.TextField("Word:", wordData.Word);
                    var newWordDef = EditorGUILayout.TextField("Definition:", wordData.Definition);
                    var newWorldLocale = (Locale)EditorGUILayout.EnumPopup("Locale:", wordData.Locale);
                    var newWordDifficulty = (WordDifficulty)EditorGUILayout.EnumPopup("Difficulty:", wordData.Difficulty);
                    if (wordData.Word != newWord || wordData.Definition != newWordDef || wordData.Difficulty != newWordDifficulty) 
                        _wordDatabase.Words[wordIndex] =  new WordData(newWord, newWordDef, newWorldLocale, newWordDifficulty);
                    
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawImportSettings()
        {
            // Header
            EditorGUILayout.Space(10);
            EditorGUIExtensions.Line(2);
            EditorGUILayout.LabelField("Import", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            // Settings
            _locale = (Locale)EditorGUILayout.EnumPopup(_locale);
            _difficulty = (WordDifficulty)EditorGUILayout.EnumPopup(_difficulty);
            
            // Import btn
            if (GUILayout.Button("Import Selected Language & Difficulty", GUILayout.Height(45))) ImportWords();
        }

        private void ImportWords()
        {
            _wordDatabase.Clear();
            
            string importPath = $"Words/{_locale}";
            var wordsForLanguageTxtAsset = Resources.Load<TextAsset>(importPath);
            if (wordsForLanguageTxtAsset == null)
            {
                Debug.LogError($"Failed to import {_locale}, does the file exist in the editor resources?");
                return;
            }
            
            string wordsForLanguageTxt = wordsForLanguageTxtAsset.ToString();
            
            string[] stringSeparators = { "\n" };
            string[] lines = wordsForLanguageTxt.Split(stringSeparators, StringSplitOptions.None);
            foreach (string line in lines)
            {
                // remove spaces
                var word = line.Replace(" ", "");
                
                // Get length
                var wordLength = word.Length;

                switch (_difficulty)
                {
                    // Filter based on import settings...
                    case WordDifficulty.Easy when wordLength > 5:
                    case WordDifficulty.Medium when wordLength is < 5 or > 10:
                    case WordDifficulty.Hard when wordLength is < 10 or > 15:
                    case WordDifficulty.VeryHard when wordLength < 15: continue;
                    // Add to database...
                    default:
                    {
                        WordDictionaryEntry definition = WordDictionary.Lookup(word);
                        _wordDatabase.Add(new WordData(word, definition.ToString().Replace($"{word}: ", ""), _locale, _difficulty));
                        break;
                    }
                }
            }
            
            EditorUtility.SetDirty(_wordDatabase);
        }
    }
}
