using System;
using System.Collections.Generic;
using System.Linq;
using KeyboardDefense.Services;
using Konfus.Utility.Extensions;

namespace KeyboardDefense.UI
{
    public class PromptFocusManager : GameService<IPromptFocusManager>, IPromptFocusManager
    {
        private readonly PromptTextBox _lastPromptToRequestFocus;
        private readonly List<(PromptTextBox promptUI, int priority)> _focusedPrompts = new();

        public void Focus(PromptTextBox promptTextBox, int priority = Int32.MaxValue)
        {
            _focusedPrompts.ToList().ForEach(p => ClearFocus(p.promptUI));
            _focusedPrompts.Add((promptTextBox, priority));
            promptTextBox.Focus();
        }

        public void RequestFocus(PromptTextBox promptTextBox, int priority)
        {
            if (CanPromptGrabFocus(promptTextBox, priority))
            {
                _focusedPrompts.ToList().ForEach(p => ClearFocus(p.promptUI));
                _focusedPrompts.Add((promptTextBox, priority));
                promptTextBox.Focus();
            }
        }

        public void ClearFocus(PromptTextBox promptTextBox)
        {
            _focusedPrompts.RemoveAll(p => p.promptUI == promptTextBox);
            promptTextBox.Unfocus();
        }

        private bool CanPromptGrabFocus(PromptTextBox promptTextBox, int priority)
        {
            return _lastPromptToRequestFocus == promptTextBox ||
                   _focusedPrompts.IsNullOrEmpty() || 
                   _focusedPrompts.Any(p => p.priority == priority);
        }
    }
}
