using KeyboardDefense.Localization;
using UnityEditor;

namespace KeyboardDefense.Prompts
{
    public struct PromptData
    {
        public PromptData(string w, string def, Locale locale)
        {
            Word = w;
            Definition = def;
            Locale = locale;
        }

        public Locale Locale { get; }
        public string Word { get; }
        public string Definition { get; }
    }
}