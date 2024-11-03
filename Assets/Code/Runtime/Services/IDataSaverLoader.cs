namespace KeyboardDefense.Services
{
    public interface IDataSaverLoader : IGameService
    {
        T LoadData<T>(string key);
        void SaveData<T>(string key, T data);
    }
}