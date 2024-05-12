using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class GameplayCanvasProvider : GameService<IGameplayCanvasProvider>, IGameplayCanvasProvider
    {
        public Canvas GameplayCanvas { get; private set; }

        private void Awake()
        {
            GameplayCanvas = GetComponent<Canvas>();
        }
    }
}