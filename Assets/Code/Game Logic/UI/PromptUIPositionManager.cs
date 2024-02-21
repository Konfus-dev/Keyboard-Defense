using System.Collections.Generic;
using System.Linq;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PromptUIPositionManager : Singleton<PromptUIPositionManager>
    {
        [SerializeField]
        private float minSpacingBetweenPrompts;
        
        private Dictionary<PromptBinding, RectTransform> _promptBindingRectDict;
        private Dictionary<PromptBinding, RectTransform> _promptsToFindSpaceFor;
        private HashSet<PromptBinding> _settledPrompts;

        public void RegisterNewPrompt(GameObject prompt)
        {
            _promptBindingRectDict.Add(
                prompt.GetComponent<PromptBinding>(), prompt.GetComponent<RectTransform>());
        }
        
        private void Start()
        {
            _settledPrompts = new HashSet<PromptBinding>();
            _promptsToFindSpaceFor = new Dictionary<PromptBinding, RectTransform>();
            _promptBindingRectDict = new Dictionary<PromptBinding, RectTransform>();
        }

        private void Update()
        {
            foreach (KeyValuePair<PromptBinding, RectTransform> promptBindingRectKeyValPair in _promptBindingRectDict)
            {
                PromptBinding promptBinding = promptBindingRectKeyValPair.Key;
                RectTransform promptRect = promptBindingRectKeyValPair.Value;
                
                if (_settledPrompts.Contains(promptBinding)) continue;
                
                if (DoesPositionOverlapWithPrompt(promptRect.position, overlappingPrompt: out RectTransform _))
                {
                    _promptsToFindSpaceFor.Add(promptBinding, promptRect);
                }
            }
        }

        private void LateUpdate()
        {
            Dictionary<PromptBinding, RectTransform> promptsToFindSpaceFor = _promptsToFindSpaceFor.ToDictionary(
                kv => kv.Key, kv => kv.Value);
            foreach (KeyValuePair<PromptBinding, RectTransform> promptBindingRectKeyValPair in promptsToFindSpaceFor)
            {
                PromptBinding promptBinding = promptBindingRectKeyValPair.Key;
                RectTransform promptRect = promptBindingRectKeyValPair.Value;

                promptBinding.SetFollowOffset(FindOpenSpace(promptRect));
                
                _promptsToFindSpaceFor.Remove(promptBinding);
                _settledPrompts.Add(promptBinding);
            }
        }

        private Vector3 FindOpenSpace(RectTransform prompt)
        {
            bool foundPos = false;
            var tentativePos = prompt.position;
            
            while (!foundPos)
            {
                tentativePos += Vector3.up * prompt.rect.height;

                if (!DoesPositionOverlapWithPrompt(tentativePos, out RectTransform overlappingPrompt))
                {
                    foundPos = true;
                    continue;
                }

                tentativePos = overlappingPrompt.position;
            }

            return tentativePos;
        }

        private bool DoesPositionOverlapWithPrompt(Vector3 posToCheck, out RectTransform overlappingPrompt)
        {
            foreach (var promptBindingRectKeyValPair in _promptBindingRectDict)
            {
                RectTransform prompt = promptBindingRectKeyValPair.Value;
                
                // Calculate the distance between the two UI elements
                float distance = Vector2.Distance(posToCheck, prompt.position);
                    
                // If the distance is less than the sum of their widths plus the spacing then they are overlapping
                if (distance < (prompt.rect.width + minSpacingBetweenPrompts))
                {
                    overlappingPrompt = prompt;
                    return true;
                }
            }

            overlappingPrompt = null;
            return false;
        }
    }
}
