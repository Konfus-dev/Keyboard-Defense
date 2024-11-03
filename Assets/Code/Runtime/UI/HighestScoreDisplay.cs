using KeyboardDefense.Serialization;
using KeyboardDefense.Services;
using TMPro;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class HighestScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text highScoreText;

        private void Start()
        {
            var dataSaverLoader = ServiceProvider.Get<IDataSaverLoader>();
            var highScore = dataSaverLoader.LoadData<HighScoreSaveData>("HighScore")?.highScore ?? 0;
            highScoreText.text = highScore.ToString();
        }
    }
}
