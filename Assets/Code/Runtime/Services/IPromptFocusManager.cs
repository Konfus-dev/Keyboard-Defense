using System;
using KeyboardDefense.UI;

namespace KeyboardDefense.Services
{
    public interface IPromptFocusManager : IGameService
    {
        void Focus(PromptUI promptUI, int priority = Int32.MaxValue);
        void RequestFocus(PromptUI promptUI, int priority);
        void ClearFocus(PromptUI promptUI);
    }
}