using KeyboardDefense.Score;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Characters
{
    [RequireComponent(typeof(Character))]
    public class AddToScoreOnDeath : MonoBehaviour
    {
        private IScoreManager _scoreManager;
        private Character _character;
        
        private void Awake()
        {
            _character = GetComponent<Character>();
        }
        
        private void Start()
        {
            _scoreManager = ServiceProvider.Get<IScoreManager>();
            _character.onDie.AddListener(AddToScoreWhenCharacterDies);
        }

        private void AddToScoreWhenCharacterDies()
        {
            _scoreManager.AddToScore(((int)_character.GetStats().Difficulty) + 1);
        }
    }
}