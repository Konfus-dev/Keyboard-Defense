using KeyboardDefense.Input.Input;

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