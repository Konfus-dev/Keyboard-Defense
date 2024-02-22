using System.Collections.Generic;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.UI
{
    /*public class PromptUIPositionManager : Singleton<PromptUIPositionManager>
    {
        [SerializeField]
        private float timeBetweenChecksToPositionPrompts = 1f;
        [SerializeField]
        private float minSpacingBetweenPrompts;
        
        private List<PromptPositioningData> _positioningData;
        private HashSet<RectTransform> _settledPrompts;
        private float _timer;

        public void RegisterNewPrompt(GameObject prompt)
        {
            var posData = new PromptPositioningData(
                prompt.GetComponent<PromptBinding>(),
                prompt.GetComponent<RectTransform>());
            _positioningData.Add(posData);
        }
        
        private void Start()
        {
            _positioningData = new List<PromptPositioningData>();
            _settledPrompts = new HashSet<RectTransform>();
        }

        private void Update()
        {
            // A timer to only check every few seconds (or whatever time interval we set)
            _timer += Time.deltaTime;
            if (!(_timer >= timeBetweenChecksToPositionPrompts)) return;
            
            // Time to check! See if any of our prompts are overlapping
            PromptPositioningData? overlappingPrompt = FindOverlappingPrompt();
            if (overlappingPrompt != null)
            {
                // Oh no! One is overlapping, lets move it :)
                PositionOverlappingPromptToNotOverlap(overlappingPrompt.Value);
            }
            // Reset timer...
            _timer = 0f;
        }

        private void PositionOverlappingPromptToNotOverlap(PromptPositioningData posData)
        {
            PromptBinding promptBinding = posData.Binding;
            RectTransform promptRect = posData.RectTransform;
            
            // Check if we are overlapping
            if (TryFindPromptThatOverlaps(promptRect, out RectTransform _))
            {
                // The prompt overlaps another prompt, get an offset that will make them not overlap...
                Vector2 offset = FindOffsetThatDoesntOverlap(
                    promptRect.position, promptBinding.GetFollowOffset());
                
                // Move to non-overlapping pos and 'lock' the prompts position by adding it to our settled list
                // TODO: we need to clean up this settled list when a prompt is re-spawned (using an obj pool)
                promptBinding.SetAnchorOffset(offset);
                _settledPrompts.Add(promptRect);
            }
        }
        
        private PromptPositioningData? FindOverlappingPrompt()
        {
            foreach (var posData in _positioningData)
            {
                RectTransform promptRect = posData.RectTransform;
                
                if (_settledPrompts.Contains(promptRect)) continue;
                
                if (TryFindPromptThatOverlaps(promptRect, out RectTransform _))
                {
                    return posData;
                }
            }

            return null;
        }

        private Vector2 FindOffsetThatDoesntOverlap(Vector3 position, Vector2 currOffset)
        {
            int gridYBound = Screen.height;
            int gridXBound = Screen.width;
            int cellSize = 100;
            
            bool foundPos = false;
            Vector2 nextCellStep = Vector2.up * cellSize;
            Vector2 nextCellToCheckForAvailability = currOffset;
            
            // Iterate over a 'grid' of positions and find an open 'cell' to move to
            // The grid is a bunch of 'cells' that span the screen who's size is the prompts width
            // TODO: take the actual width of the prompts for the 'cell size', rn just using hard coded value of 100
            while (!foundPos)
            {
                nextCellToCheckForAvailability += nextCellStep;
                var worldPosToCheckForAvailability = position + new Vector3(nextCellToCheckForAvailability.x, nextCellToCheckForAvailability.y, 0);
                
                // We are above the screen bounds!
                if (worldPosToCheckForAvailability.y >= gridYBound)
                {
                    nextCellStep = (Vector2.left * cellSize);
                    nextCellToCheckForAvailability += nextCellStep;
                    nextCellToCheckForAvailability.y = cellSize;
                    worldPosToCheckForAvailability = position + new Vector3(nextCellToCheckForAvailability.x, nextCellToCheckForAvailability.y, 0);
                    nextCellStep = (Vector2.up * cellSize);
                }
                /#1#/ We are below the screen bounds!
                else if (worldPosToCheckForAvailability.y <= 0)
                {
                    nextCellStep = Vector2.up * cellSize + (Vector2.left * cellSize);
                    nextCellToCheckForAvailability += nextCellStep;
                    worldPosToCheckForAvailability = position + new Vector3(nextCellToCheckForAvailability.x, nextCellToCheckForAvailability.y, 0);
                    nextCellStep = Vector2.up * cellSize;
                }#1#
                
                // Have we found an open 'cell'?
                if (!TryFindPromptThatOverlapsWithPos(worldPosToCheckForAvailability, out _))
                {
                    // We have! Set search condition to true..
                    foundPos = true;
                }
            }
            
            // Return open pos we have found!
            return nextCellToCheckForAvailability;
        }

        private bool TryFindPromptThatOverlaps(RectTransform rectTransform, out RectTransform overlappingPrompt)
        {
            foreach (var promptBindingRectKeyValPair in _positioningData)
            {
                RectTransform promptRectToCheckForOverlap = promptBindingRectKeyValPair.RectTransform;
                if (rectTransform == promptRectToCheckForOverlap) continue;

                if (DoesPositionOverlapWithAnyPrompts(rectTransform.position, promptRectToCheckForOverlap))
                {
                    overlappingPrompt = promptRectToCheckForOverlap;
                    return true;
                }
            }

            overlappingPrompt = null;
            return false;
        }
        
        private bool TryFindPromptThatOverlapsWithPos(Vector2 posToCheck, out RectTransform overlappingPrompt)
        {
            foreach (var promptBindingRectKeyValPair in _positioningData)
            {
                RectTransform prompt = promptBindingRectKeyValPair.RectTransform;
                if (DoesPositionOverlapWithAnyPrompts(posToCheck, prompt))
                {
                    overlappingPrompt = prompt;
                    return true;
                }
            }

            overlappingPrompt = null;
            return false;
        }

        private bool DoesPositionOverlapWithAnyPrompts(Vector2 position, RectTransform rectTransform)
        {
            // Calculate the distance between the two UI elements
            float distance = Vector2.Distance(position, rectTransform.position);
                    
            // If the distance is less than the sum of their widths plus the spacing then they are overlapping
            if (distance < (rectTransform.rect.width + minSpacingBetweenPrompts))
            {
                return true;
            }

            return false;
        }
        
        private struct PromptPositioningData
        {
            public PromptPositioningData(PromptBinding binding, RectTransform rectTransform)
            {
                Binding = binding;
                RectTransform = rectTransform;
            }

            public RectTransform RectTransform { get; }
            public PromptBinding Binding { get; }
        }
    }*/
}
