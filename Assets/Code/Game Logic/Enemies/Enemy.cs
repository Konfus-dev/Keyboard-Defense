using System.Collections;
using KeyboardCats.Prompts;
using KeyboardCats.Vitality;
using UnityEngine;

namespace KeyboardCats.Enemies
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private PromptDifficulty difficulty;
        [SerializeField] 
        private MonoPrompt prompt;
        [SerializeField]
        private float attackDamage = 1f;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField] 
        private float moveSpeed = 1f;
        [SerializeField]
        private Health health;
        [SerializeField]
        private RaycastAttack raycastAttack;
        [SerializeField]
        private SplineMovement splineMovement;

        private State _state = State.Moving;

        public State GetState()
        {
            return _state;
        }

        public Health GetHealth()
        {
            return health;
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public void OnPromptCompleted()
        {
            health.TakeDamage(int.MaxValue);
        }

        public void OnHitCastle()
        {
            Attack();
        }

        public void OnTakeDamage()
        {
            Hurt();
        }
        
        public void OnHealthHitZero()
        {
            Die();
        }

        private void Start()
        {
            prompt.Generate(difficulty);
            splineMovement.SetContainer(EnemyManager.Instance.GetPath());
            splineMovement.SetSpeed(moveSpeed);
            splineMovement.Move();
        }

        private void Attack()
        {
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            _state = State.Attacking;
            splineMovement.Stop();
            
            while (_state == State.Attacking)
            {
                _state = State.Attacking;
                raycastAttack.Attack(attackDamage);
                yield return new WaitForSeconds(0.2f);
                _state = State.Idle;
                yield return new WaitForSeconds(attackCooldown - 0.2f);
                _state = State.Attacking;
            }
        }

        private void Hurt()
        {
            if (_state == State.Dead) return;
            StartCoroutine(HurtRoutine());
        }
        
        private IEnumerator HurtRoutine()
        {
            var previousState = _state;
            _state = State.Hurt;
            splineMovement.Stop();
            yield return new WaitForSeconds(0.5f);
            _state = previousState;
            splineMovement.Move();
        }
        
        private void Die()
        {
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            _state = State.Dead;
            splineMovement.Stop();
            yield return new WaitForSeconds(0.5f);
            // TODO: death FX, like a poof and ghost rising
            Destroy(gameObject);
        }

        public enum State
        {
            Idle,
            Moving,
            Attacking,
            Hurt,
            Dead
        }
    }
}
