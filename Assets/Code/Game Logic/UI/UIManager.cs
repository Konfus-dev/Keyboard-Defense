using KeyboardCats.Enemies;
using Konfus.Utility.Design_Patterns;

namespace KeyboardCats.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public void OnEnemySpawned(Enemy enemy)
        {
            var promptUI = enemy.GetComponentInChildren<PromptUI>();
        }
        
        private void Update()
        {
            
        }
    }
}