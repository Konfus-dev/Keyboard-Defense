using KeyboardDefense.UI;
using Konfus.Utility.Design_Patterns;
using Unity.Mathematics;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class PromptGenerator : MonoBehaviour
    {
        private const string PROMPT_SPAWNPOOL_KEY = "Prompt";
        
        [SerializeField]
        private PromptDifficulty promptToGenerateDifficulty;
        [SerializeField] 
        private GameObject objForGeneratedPromptToFollow;
        [SerializeField] 
        private Vector2 generatedPromptFollowOffset;

        public Prompt GeneratedPrompt => _generatedPrompt;
        
        private Prompt _generatedPrompt;
        private PromptUI _generatedPromptUI;
        
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
            GameObject promptGo = ObjectPoolManager.Instance
                .SpawnFromPool(PROMPT_SPAWNPOOL_KEY, transform.position, quaternion.identity);
            
            // Generate prompt
            string promptStr = PromptManager.Instance.GeneratePrompt(promptToGenerateDifficulty);
            _generatedPrompt = promptGo.GetComponent<Prompt>();
            _generatedPrompt.Set(promptStr);
            _generatedPromptUI = _generatedPrompt.GetComponent<PromptUI>();
            _generatedPromptUI.SetPrompt(promptStr);
            
            // Tell prompt to follow the object we are configured to follow
            var uiFollowObjectInWorld = _generatedPrompt.GetComponent<UIFollowObjectInWorld>();
            uiFollowObjectInWorld.SetObjectToFollow(objForGeneratedPromptToFollow);
            uiFollowObjectInWorld.SetFollowOffset(generatedPromptFollowOffset);
            
        }
    }
}
