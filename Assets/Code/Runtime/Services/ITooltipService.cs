namespace KeyboardDefense.Services
{
    public interface ITooltipService : IGameService
    {
        void Show(string contentTxt, string headerTxt = "");
        void Hide();
    }
}