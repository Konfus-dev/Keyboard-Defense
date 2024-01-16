using System;
using System.Collections;
using System.Linq;
using KeyboardCats.Data;
using Konfus.Utility.Extensions;
using UnityEngine;

namespace KeyboardCats.UI
{
    public class PromptUI : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField]
        private GameObject wordPrefab;
        [SerializeField]
        private RectTransform root;
        [SerializeField] 
        private Transform anchor;
        [SerializeField] 
        private RectTransform tail;
        [SerializeField] 
        private Canvas canvas;
        [SerializeField] 
        private DefinitionUI definitionUI;
        [SerializeField] 
        private MouseEventListener mouseEventListener;
        
        private WordUI[] _words;

        public void Initialize(PromptData data)
        {
            StartCoroutine(InitializeRoutine(data));
        }

        public void OnFailedToTypePrompt()
        {
            StartCoroutine(_words.First().OnFailedToTypeWordRoutine());
        }

        public void OnNextCharInPromptTyped()
        {
            if (_words.First() == null)
            {
                _words = _words.RemoveAt(0);
            }

            _words.First().OnNextCharInWordTyped();
        }
        
        private IEnumerator InitializeRoutine(PromptData prompt)
        {
            // Remove any old text
            if (_words != null)
            {
                foreach (var word in _words)
                {
                    Destroy(word.gameObject);
                }
            }
            
            // Update prompt UI
            _words = new WordUI[prompt.Words.Length];
            for (int wordIndex = 0; wordIndex < _words.Length; wordIndex++)
            {
                var wordUIGo = Instantiate(wordPrefab, root);
                var wordUI = wordUIGo.GetComponent<WordUI>();
                _words[wordIndex] = wordUI;
                yield return wordUI.InitializeRoutine(
                    prompt.Words[wordIndex],
                    mouseEventListener,
                    definitionUI);
            }
        }

        private void Awake()
        {
            _words = GetComponentsInChildren<WordUI>();
        }

        private void Start()
        {
            canvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            //KeepInScreenBounds();
            UpdateTail();
        }

        private bool IsOutOfScreenBounds(out Vector3 clampedPos)
        {
            Camera mainCamera = Camera.main;

            // Convert the world position to screen coordinates
            Vector3 screenPosLeft = mainCamera.WorldToScreenPoint(transform.position - (Vector3.right * 5));
            Vector3 screenPosRight = mainCamera.WorldToScreenPoint(transform.position + (Vector3.right * 5));

            // Get the screen boundaries
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            // Clamp the screen position to be within the screen boundaries
            float clampedX = Mathf.Clamp(screenPosLeft.x, 0, screenWidth);
            float clampedY = Mathf.Clamp(screenPosLeft.y, 0, screenHeight);


            // Convert the clamped screen position back to world coordinates
            clampedPos = mainCamera.ScreenToWorldPoint(new Vector3(clampedX, clampedY, screenPosLeft.z));

            // Check if the object is outside the screen boundaries
            return (screenPosLeft.x < 0 || screenPosLeft.x > screenWidth || screenPosLeft.y < 0 || screenPosLeft.y > screenHeight) || 
                   (screenPosRight.x < 0 || screenPosRight.x > screenWidth || screenPosRight.y < 0 || screenPosRight.y > screenHeight);
        }

        private void KeepInScreenBounds()
        {
            // Check if the GameObject is out of bounds
            if (IsOutOfScreenBounds(out var clampedPos))
            {
                transform.position = clampedPos;
            }
        }
        
        private void UpdateTail()
        {
            var scale = tail.localScale;
            tail.localScale =  new Vector3(scale.x, (root.transform.position - anchor.transform.position).magnitude, scale.z);
        }
        
    }
}