using System.IO;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Serialization
{
    public class SaveDataManager : GameService<IDataSaverLoader>, IDataSaverLoader
    {
        public T LoadData<T>(string key)
        {
            var savePath = Path.Combine(Application.persistentDataPath, $"{key}.json");// Check if the save file exists
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log("Data loaded from " + savePath);
                return data;
            }

            Debug.LogWarning("Save file not found at " + savePath);
            return default;
        }

        public void SaveData<T>(string key, T data)
        {
            var savePath = Path.Combine(Application.persistentDataPath, $"{key}.json");
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, json);
            Debug.Log("Data saved to " + savePath);
        }
    }
}