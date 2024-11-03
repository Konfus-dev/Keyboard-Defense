using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Characters
{
    public abstract class Character : MonoBehaviour, IHasHealth
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
        private int _currentHealth;
        private bool _invulnerable;

        public State GetState()
        {
            return _runningState;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_invulnerable) return;
            if (_runningState == State.Dead) return;
            SetState(State.Hurt);
            if (_currentHealth <= 0) SetState(State.Dead);
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public CharacterStats GetStats()
        {
            return stats;
        }

        protected void SetState(State state)
        {
            if (_runningState == state) return;
            
            _previousState = _runningState;
            _runningState = state;
            Debug.Log($"{name} is: {_runningState}");
            
            UpdateState();
        }
        
        protected virtual void OnSpawn() 
        { 
            _currentHealth = stats.Health;
            StartCoroutine(SpawnInvulnerabilityTimerRoutine());
        }

        protected virtual void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            OnSpawn();
        }
        
        private void UpdateState()
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
        }
        
        private void Move()
        {
            _runningState = State.Moving;
            onMove.Invoke();
        }
        
        private void Idle()
        {
            _runningState = State.Idle;
            onIdle.Invoke();
        }

        private void Attack()
        {
            _runningState = State.Attacking;
            StartCoroutine(AttackRoutine());
        }

        private void Hurt()
        {
            _runningState = State.Hurt;
            StartCoroutine(HurtRoutine());
        }

        private void Die()
        {
            _runningState = State.Dead;
            StartCoroutine(DieRoutine());
        }
        
        private IEnumerator AttackRoutine()
        {
            while (_runningState == State.Attacking)
            {
                onAttack.Invoke();
                yield return new WaitForSeconds(stats.AttackCooldown);
            }
        }
        
        private IEnumerator HurtRoutine()
        {
            onHurt.Invoke();
            yield return new WaitForSeconds(stats.StunDuration);
            SetState(_previousState);
        }
        
        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            onDie.Invoke();
            OnDie();
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
