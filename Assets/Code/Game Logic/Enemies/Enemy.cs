using System;
using System.Collections;
using KeyboardCats.Prompts;
using KeyboardCats.UI;
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
        private PromptUI promptUI;
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
            _state = State.Attacking;
        }

        public void OnTakeDamage()
        {
            _state = State.Hurt;
        }
        
        public void OnHealthHitZero()
        {
            _state = State.Dead;
        }

        private void Start()
        {
            promptUI.Generate(difficulty);
            splineMovement.SetContainer(EnemyManager.Instance.GetPath());
            splineMovement.SetSpeed(moveSpeed);
            splineMovement.Move();
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Moving:
                    Move();
                    break;
                case State.Attacking:
                    Attack();
                    break;
                case State.Idle:
                    Idle();
                    break;
                case State.Hurt:
                    Hurt();
                    break;
                case State.Dead:
                    Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"No implementation for {_state}");
            }
        }

        private void Move()
        {
            splineMovement.SetSpeed(moveSpeed);
            splineMovement.Move();
        }
        
        private void Idle()
        {
            splineMovement.SetSpeed(0f);
            splineMovement.Stop();
        }

        private void Attack()
        {
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
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
            splineMovement.Stop();
            var previousState = _state;
            yield return new WaitForSeconds(0.5f);
            _state = previousState;
        }
        
        private void Die()
        {
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
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
