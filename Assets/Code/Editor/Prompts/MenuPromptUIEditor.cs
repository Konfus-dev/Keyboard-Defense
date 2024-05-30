using KeyboardDefense.UI;
using TMPro;
using UnityEditor;

namespace KeyboardDefense.Editor.Prompts
{
    [CustomEditor(typeof(MenuPrompt))]
    public class MenuPromptUIEditor : UnityEditor.Editor
    {
        private string _lastTextValue;
        private SerializedProperty _uiText;
        private PromptTextBox _promptTextBox;
        private TMP_Text _promptUIText;

        public override void OnInspectorGUI()
        {
            // Display default inspector GUI
            DrawDefaultInspector();

            // Update UI if text changed
            if (_uiText != null && _uiText.serializedObject.targetObject != null && _uiText.stringValue != _lastTextValue)
            {
                _promptUIText.text = _uiText.stringValue;
                _lastTextValue = _uiText.stringValue;
               // _promptTextBox.SetSize(_promptUIText.text.Length + 1);
            }
        }

        private void OnEnable()
        {
            var menuPromptUI = (MenuPrompt)target;
            _promptTextBox = menuPromptUI.GetComponent<PromptTextBox>();
            _promptUIText = menuPromptUI.GetComponentInChildren<TMP_Text>();
            _uiText = serializedObject.FindProperty("text");
        }
    }
}