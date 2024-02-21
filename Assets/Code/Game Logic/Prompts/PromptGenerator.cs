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
        private GameObject objToBindTo;
        [SerializeField] 
        private Vector2 followOffset;

        private PromptBinding _binding;
        
        private void OnEnable()
        {
            GeneratePrompt();
        }
        
        private void OnDisable()
        {
            if (_binding)
            {
                _binding.gameObject.SetActive(false);
            }
        }

        private void GeneratePrompt()
        {
            GameObject prompt = ObjectPoolManager.Instance
                .SpawnFromPool(PROMPT_SPAWNPOOL_KEY, transform.position, quaternion.identity);
            _binding = prompt.GetComponent<PromptBinding>();
            _binding.Reset();
            _binding.SetObjToFollow(objToBindTo);
            _binding.SetFollowOffset(followOffset);
        }
    }
}
