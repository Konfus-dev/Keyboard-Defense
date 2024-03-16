using System.Collections.Generic;
using System.Linq;
using KeyboardDefense.Services;
using Konfus.Utility.Extensions;

namespace KeyboardDefense.UI
{
    public class PromptFocusManager : GameService<IPromptFocusManager>, IPromptFocusManager
    {
        private readonly List<(PromptUI promptUI, int priority)> _focusedPrompts = new();
    
        public void RequestFocus(PromptUI promptUI, int priority)
        {
            var promptsToUnfocus =_focusedPrompts.FindAll(p => p.priority < priority);
            promptsToUnfocus.ForEach(p => ClearFocus(p.promptUI));
            if (CanPromptGrabFocus(priority))
            {
                _focusedPrompts.Add((promptUI, priority));
                promptUI.Focus();
            }
        }

        public void ClearFocus(PromptUI promptUI)
        {
            _focusedPrompts.RemoveAll(p => p.promptUI == promptUI);
            promptUI.Unfocus();
        }

        private bool CanPromptGrabFocus(int priority)
        {
            return _focusedPrompts.IsNullOrEmpty() || _focusedPrompts.Any(p => p.priority == priority);
        }
    }
}
