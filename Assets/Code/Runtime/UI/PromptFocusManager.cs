using System;
using System.Collections.Generic;
using System.Linq;
using KeyboardDefense.Services;
using Konfus.Utility.Extensions;

namespace KeyboardDefense.UI
{
    public class PromptFocusManager : GameService<IPromptFocusManager>, IPromptFocusManager
    {
        private readonly PromptUI _lastPromptToRequestFocus;
        private readonly List<(PromptUI promptUI, int priority)> _focusedPrompts = new();

        public void Focus(PromptUI promptUI, int priority = Int32.MaxValue)
        {
            _focusedPrompts.ToList().ForEach(p => ClearFocus(p.promptUI));
            _focusedPrompts.Add((promptUI, priority));
            promptUI.Focus();
        }

        public void RequestFocus(PromptUI promptUI, int priority)
        {
            if (CanPromptGrabFocus(promptUI, priority))
            {
                _focusedPrompts.ToList().ForEach(p => ClearFocus(p.promptUI));
                _focusedPrompts.Add((promptUI, priority));
                promptUI.Focus();
            }
        }

        public void ClearFocus(PromptUI promptUI)
        {
            _focusedPrompts.RemoveAll(p => p.promptUI == promptUI);
            promptUI.Unfocus();
        }

        private bool CanPromptGrabFocus(PromptUI promptUI, int priority)
        {
            return _lastPromptToRequestFocus == promptUI ||
                   _focusedPrompts.IsNullOrEmpty() || 
                   _focusedPrompts.Any(p => p.priority == priority);
        }
    }
}
