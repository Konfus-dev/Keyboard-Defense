using KeyboardDefense.Prompts;
using UnityEngine;

namespace KeyboardDefense.Characters
{
    [CreateAssetMenu(fileName = "New Character Stats", menuName = "Keyboard Defense/New Character Stats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField]
        private WordDifficulty difficulty;
        [SerializeField] 
        private int health = 2;
        [SerializeField]
        private int attackDamage = 1;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField]
        private float stunDuration = 0.5f;
        [SerializeField] 
        private float moveSpeed = 1f;

        public WordDifficulty Difficulty => difficulty;
        public int Health => health;
        public int AttackDamage => attackDamage;
        public float AttackCooldown => attackCooldown;
        public float StunDuration => stunDuration;
        public float MoveSpeed => moveSpeed;
    }
}