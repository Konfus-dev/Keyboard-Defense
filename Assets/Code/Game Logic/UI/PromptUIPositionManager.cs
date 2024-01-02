using System.Collections.Generic;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardCats.UI
{
    public class PromptUIPositionManager : Singleton<PromptUIPositionManager>
    {
        [SerializeField] private float padding = 10f;
        
        private List<RectTransform> _uiObjects;

        public void Register(RectTransform uiObj)
        {
            _uiObjects.Add(uiObj);
        }
        
        public void UnRegister(RectTransform uiObj)
        {
            _uiObjects.Remove(uiObj);
        }
        
        private void Start()
        {
            _uiObjects = new List<RectTransform>();
        }

        private void Update()
        {
            ArrangePromptUIObjects();
        }

        private void ArrangePromptUIObjects()
        {
            var uiObjects = _uiObjects;
            for (var i = 0; i < uiObjects.Count; i++)
            {
                for (var j = 0; j < uiObjects.Count; j++)
                {
                    if (i == j) continue;
                    
                    var rectTransformA = uiObjects[i];
                    var rectTransformB = uiObjects[j];

                    // Check for overlap
                    if (!RectOverlaps(rectTransformA, rectTransformB)) continue;
                    
                    // Move the second UI object to avoid overlap
                    var offset = CalculateOffset(rectTransformA, rectTransformB);
                    rectTransformB.anchoredPosition += offset + new Vector2(padding, padding);
                }
            }
        }

        private bool RectOverlaps(RectTransform rectA, RectTransform rectB)
        {
            var rect1 = new Rect(rectA.anchoredPosition.x, rectA.anchoredPosition.y, rectA.sizeDelta.x, rectA.sizeDelta.y);
            var rect2 = new Rect(rectB.anchoredPosition.x, rectB.anchoredPosition.y, rectB.sizeDelta.x, rectB.sizeDelta.y);
            return rect1.Overlaps(rect2);
        }

        private Vector2 CalculateOffset(RectTransform rectA, RectTransform rectB)
        {
            var distanceX = rectA.position.x - rectA.sizeDelta.x / 2f - (rectB.position.x + rectB.sizeDelta.x / 2f);
            var distanceY = rectA.position.y - rectA.sizeDelta.y / 2f - (rectB.position.y + rectB.sizeDelta.y / 2f);

            return new Vector2(distanceX, distanceY);
        }
    }
}