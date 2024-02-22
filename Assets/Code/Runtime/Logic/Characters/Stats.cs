using KeyboardDefense.Logic.Prompts;
using UnityEngine;

namespace KeyboardDefense.Logic.Characters
{
    [CreateAssetMenu(fileName = "New Character Stats", menuName = "Keyboard Defense/New Character Stats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField]
        private PromptDifficulty difficulty;
        [SerializeField] 
        private float health = 2f;
        [SerializeField]
        private float attackDamage = 1f;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField]
        private float stunDuration = 0.5f;
        [SerializeField] 
        private float moveSpeed = 1f;

        public PromptDifficulty Difficulty => difficulty;
        public float Health => health;
        public float AttackDamage => attackDamage;
        public float AttackCooldown => attackCooldown;
        public float StunDuration => stunDuration;
        public float MoveSpeed => moveSpeed;
    }
}