using KeyboardDefense.UI;

namespace KeyboardDefense.Services
{
    public interface ICursorService
    {
        CursorState GetCursorState();
        CursorState GetLastCursorState();
        void SetCursor(CursorState cursorState);
    }
}