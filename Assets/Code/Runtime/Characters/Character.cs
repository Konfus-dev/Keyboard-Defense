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

        protected void SetState(State state)
        {
            if (_runningState == state) return;
            
            _previousState = _runningState;
            _runningState = state;
            Debug.Log($"{name} is: {_runningState}");
            
            UpdateState();
        }

        private void OnEnable()
        {
            OnSpawn();
        }
        
        protected virtual void OnSpawn() 
        { 
            _currentHealth = stats.Health;
        }

        protected virtual void OnUpdate()
        {
        }
        
        private void Update()
        {
            OnUpdate();
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
            onMove.Invoke();
        }
        
        private void Idle()
        {
            onIdle.Invoke();
        }

        private void Attack()
        {
            StartCoroutine(AttackRoutine());
        }

        private void Hurt()
        {
            StartCoroutine(HurtRoutine());
        }

        private void Die()
        {
            StartCoroutine(DieRoutine());
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
            gameObject.SetActive(false);
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
