using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Characters
{
    public class Character : MonoBehaviour, IHasHealth
    {
        public UnityEvent onDie;
        public UnityEvent onIdle;
        public UnityEvent onHurt;
        public UnityEvent onAttack;
        public UnityEvent onMove;
        
        [SerializeField, Space]
        private CharacterStats stats;

        private State _runningState;
        private State _previousState;
        private float _currentHealth;
        private bool _invulnerable;

        public State GetState()
        {
            return _runningState;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_invulnerable) return;
            if (_runningState == State.Dead) return;
            SetState(State.Hurt);
            if (_currentHealth <= 0) SetState(State.Dead);
        }

        public float GetCurrentHealth()
        {
            return _currentHealth;
        }

        public CharacterStats GetStats()
        {
            return stats;
        }
        
        protected void Move()
        {
            _runningState = State.Moving;
            onMove.Invoke();
        }
        
        protected void Idle()
        {
            _runningState = State.Idle;
            onIdle.Invoke();
        }

        protected void Attack()
        {
            if (_runningState == State.Attacking) return;
            StartCoroutine(AttackRoutine());
        }

        protected void Hurt()
        {
            if (_runningState == State.Hurt) return;
            StartCoroutine(HurtRoutine());
        }

        protected void Die()
        {
            if (_runningState == State.Dead) return;
            StartCoroutine(DieRoutine());
        }
        
        protected virtual void OnSpawn() 
        { 
            _currentHealth = stats.Health;
            SetState(State.Moving);
        }
        
        protected virtual void OnDie() 
        { 
        }

        private void OnEnable()
        {
            OnSpawn();
        }

        private void Update()
        {
            switch (_runningState)
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
                    throw new ArgumentOutOfRangeException($"No implementation for {_runningState}");
            }
            
            Debug.Log($"{name} is: {_runningState}");
        }

        private void SetState(State state)
        {
            _previousState = _runningState;
            _runningState = state;
        }
        
        private IEnumerator AttackRoutine()
        {
            _runningState = State.Attacking;
            while (_runningState == State.Attacking)
            {
                onAttack.Invoke();
                yield return new WaitForSeconds(stats.AttackCooldown);
            }
        }
        
        private IEnumerator HurtRoutine()
        {
            _runningState = State.Hurt;
            onHurt.Invoke();
            yield return new WaitForSeconds(stats.StunDuration);
            SetState(_previousState);
        }
        
        private IEnumerator DieRoutine()
        {
            _runningState = State.Dead;
            yield return new WaitForSeconds(0.5f);
            onDie.Invoke();
            OnDie();
            Destroy(gameObject);
        }

        private IEnumerator SpawnInvulnerabilityTimerRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            _invulnerable = false;
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
