namespace KeyboardDefense.Services
{
    public interface ITooltipService
    {
        void Show(string contentTxt, string headerTxt = "");
        void Hide();
    }
}