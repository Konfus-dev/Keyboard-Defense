using System;
using KeyboardDefense.UI;

namespace KeyboardDefense.Services
{
    public interface IPromptFocusManager : IGameService
    {
        void Focus(PromptTextBox promptTextBox, int priority = Int32.MaxValue);
        void RequestFocus(PromptTextBox promptTextBox, int priority);
        void ClearFocus(PromptTextBox promptTextBox);
    }
}