using KeyboardDefense.Characters;
using KeyboardDefense.Services;
using KeyboardDefense.UI;
using Unity.Mathematics;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class PromptGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject promptPrefab;
        [SerializeField] 
        private GameObject objForGeneratedPromptToFollow;
        [SerializeField] 
        private Vector2 generatedPromptFollowOffset;

        public Prompt GeneratedPrompt => _generatedPrompt;
        
        private Prompt _generatedPrompt;
        private PromptTextBox _generatedPromptTextBox;
        private ISpawnService _spawnService;
        private IWordDatabase _wordDatabase;

        private void OnEnable()
        {
            // Get required services
            _wordDatabase ??= ServiceProvider.Get<IWordDatabase>();
            _spawnService ??= ServiceProvider.Get<ISpawnService>();
            
            // Then generate prompt
            GeneratePrompt();
        }
        
        private void OnDisable()
        {
            if (_generatedPromptTextBox)
            {
                _generatedPromptTextBox.gameObject.SetActive(false);
            }
        }

        private void GeneratePrompt()
        {
            // Spawn prompt prefab
            GameObject promptGo = Instantiate(promptPrefab, transform.position, quaternion.identity);
            
            // Generate prompt
            var character = GetComponent<Character>();
            PromptData promptData = _wordDatabase.GetRandomWordOfGivenDifficulty(character.GetStats().Difficulty);
            _generatedPrompt = promptGo.GetComponent<Prompt>();
            _generatedPrompt.Set(promptData);
            _generatedPromptTextBox = _generatedPrompt.GetComponent<PromptTextBox>();
            _generatedPromptTextBox.SetPrompt(promptData);
            
            // Tell prompt to follow the object we are configured to follow
            var uiFollowObjectInWorld = _generatedPrompt.GetComponent<ScreenSpaceObjFollowObjInWorld>();
            uiFollowObjectInWorld.SetObjectToFollow(objForGeneratedPromptToFollow);
            uiFollowObjectInWorld.SetFollowOffset(generatedPromptFollowOffset);
            
            // Show prompt
            promptGo.transform.GetChild(0).gameObject.SetActive(true);
            var promptTail = _generatedPrompt.GetComponentInChildren<PromptTail>();
            promptTail.SetObjectToFollow(objForGeneratedPromptToFollow);
        }
    }
}
