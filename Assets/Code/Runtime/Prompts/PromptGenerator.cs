using System;
using KeyboardDefense.Services;
using KeyboardDefense.Spawning;
using KeyboardDefense.UI;
using Konfus.Utility.Design_Patterns;
using Unity.Mathematics;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class PromptGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject promptPrefab;
        [SerializeField]
        private WordDifficulty wordToGenerateDifficulty;
        [SerializeField] 
        private GameObject objForGeneratedPromptToFollow;
        [SerializeField] 
        private Vector2 generatedPromptFollowOffset;

        public Prompt GeneratedPrompt => _generatedPrompt;
        
        private Prompt _generatedPrompt;
        private PromptUI _generatedPromptUI;
        private ISpawnService _spawnService;
        private IWordDatabase _wordDatabase;

        private void Awake()
        {
            _wordDatabase = ServiceProvider.Instance.Get<IWordDatabase>();
            _spawnService = ServiceProvider.Instance.Get<ISpawnService>();
        }

        private void OnEnable()
        {
            GeneratePrompt();
        }
        
        private void OnDisable()
        {
            if (_generatedPromptUI)
            {
                _generatedPromptUI.gameObject.SetActive(false);
            }
        }

        private void GeneratePrompt()
        {
            // Spawn prompt prefab
            GameObject promptGo = _spawnService.Spawn(promptPrefab, transform.position, quaternion.identity);
            
            // Generate prompt
            PromptData promptData = _wordDatabase.GetRandomWordOfGivenDifficulty(wordToGenerateDifficulty);
            _generatedPrompt = promptGo.GetComponent<Prompt>();
            _generatedPrompt.Set(promptData);
            _generatedPromptUI = _generatedPrompt.GetComponent<PromptUI>();
            _generatedPromptUI.SetPrompt(promptData);
            
            // Tell prompt to follow the object we are configured to follow
            var uiFollowObjectInWorld = _generatedPrompt.GetComponent<UIFollowObjectInWorld>();
            uiFollowObjectInWorld.SetObjectToFollow(objForGeneratedPromptToFollow);
            uiFollowObjectInWorld.SetFollowOffset(generatedPromptFollowOffset);
            var promptTail = _generatedPrompt.GetComponentInChildren<PromptTail>();
            promptTail.SetObjectToFollow(objForGeneratedPromptToFollow);
        }
    }
}
