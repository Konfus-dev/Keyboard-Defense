using System.Collections.Generic;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardCats.UI
{
    public class PromptPositionManager : Singleton<PromptPositionManager>
    {
        [SerializeField] private float padding = 10f;
        
        private List<Transform> _uiObjects;

        public void Register(Transform uiObj)
        {
            _uiObjects.Add(uiObj);
        }
        
        public void UnRegister(Transform uiObj)
        {
            _uiObjects.Remove(uiObj);
        }

        protected override void OnAwake()
        {
            _uiObjects = new List<Transform>();
            base.OnAwake();
        }

        private void Update()
        {
            // TODO: get this working!!!
            //ArrangePromptUIObjects();
        }

        private void ArrangePromptUIObjects()
        {
            var uiObjects = _uiObjects;
            for (var i = 0; i < uiObjects.Count; i++)
            {
                for (var j = 0; j < uiObjects.Count; j++)
                {
                    if (i == j) continue;
                    
                    var transformA = uiObjects[i];
                    var transformB = uiObjects[j];

                    // Check for overlap
                    if (!RectOverlaps(transformA, transformB)) continue;
                    
                    // Move the second UI object to avoid overlap
                    transformB.position += new Vector3(0, padding, 0);
                }
            }
        }

        private bool RectOverlaps(Transform rectA, Transform rectB)
        {
            return false;
        }
    }
}