using KeyboardDefense.UI;

namespace KeyboardDefense.Services
{
    public interface ICursorService : IGameService
    {
        CursorState GetCursorState();
        CursorState GetLastCursorState();
        void SetCursor(CursorState cursorState);
        void HideCursor();
    }
}