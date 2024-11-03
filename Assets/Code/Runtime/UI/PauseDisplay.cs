using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PauseDisplay : GameService<IPauseUI>, IPauseUI
    {
        [SerializeField]
        private GameObject pauseLayout;
        
        public void Show()
        {
            pauseLayout.SetActive(true);
        }

        public void Hide()
        {
            pauseLayout.SetActive(false);
        }
    }
}