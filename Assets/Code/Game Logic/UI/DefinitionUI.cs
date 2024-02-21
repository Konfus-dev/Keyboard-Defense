using System.Threading.Tasks;
using KeyboardCats.Data;
using Konfus.Utility.Extensions;
using TMPro;
using UnityEngine;

namespace KeyboardCats.UI
{
    public class DefinitionUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private TMP_Text titleText;
        [SerializeField]
        private TMP_Text defText;
        [SerializeField] 
        private GameObject root;

        private string _title;
        private string _definition;
        private bool _isOpen;
        private bool _isDirty;
        private bool _isLoading;

        public void Lookup(WordData data)
        {
            _title = data;
            SetDefinition(data).FireAndForget();
        }

        public bool IsOpen() => _isOpen;
        
        public void Open()
        {
            _isOpen = true;
            root.SetActive(true);
        }

        public void Close()
        {
            _isOpen = false;
            root.SetActive(false);
        }

        private void Update()
        {
            if (_isLoading)
            {
                _definition = "Loading...";
                UpdateText();
            }
            else if (_isDirty)
            {
                _isDirty = false;
                UpdateText();
            }
        }

        private async Task SetDefinition(WordData data)
        {
            Debug.Log($"Looking up: {data}...");
            _isLoading = true;
            _definition = await WordDictionary.LookupAsync(data).ContinueOnSameContext();
            _isLoading = false;
            _isDirty = true;
            Debug.Log(_definition);
        }

        private void UpdateText()
        {
            titleText.text = _title;
            defText.text = _definition;
        }
    }
}