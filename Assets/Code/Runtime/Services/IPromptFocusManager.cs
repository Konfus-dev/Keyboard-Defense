using KeyboardDefense.UI;

namespace KeyboardDefense.Services
{
    public interface IPromptFocusManager : IGameService
    {
        void RequestFocus(PromptUI promptUI, int priority);
        void ClearFocus(PromptUI promptUI);
    }
}