using KeyboardDefense.Logic.Score;
using UnityEngine;

namespace KeyboardDefense.Logic.Characters
{
    [RequireComponent(typeof(Character))]
    public class AddToScoreOnDeath : MonoBehaviour
    {
        private Character _character;
        
        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Start()
        {
            _character.onDie.AddListener(AddToScoreWhenCharacterDies);
        }

        private void AddToScoreWhenCharacterDies()
        {
            ScoreManager.Instance.AddToScore(((int)_character.GetStats().Difficulty) + 1);
        }
    }
}