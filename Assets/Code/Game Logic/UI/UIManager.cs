using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private ScoreUI scoreUI;
        public ScoreUI ScoreUI => scoreUI;
        [SerializeField]
        private HealthBarUI playerHealthUI;
        public HealthBarUI PlayerHealthUI => playerHealthUI;
    }
}