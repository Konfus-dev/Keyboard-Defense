using System;
using System.Collections;
using KeyboardCats.Data;
using KeyboardCats.Vitality;
using UnityEngine;
using UnityEngine.Splines;
using Prompt = KeyboardCats.UI.Prompt;

namespace KeyboardCats.Enemies
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        // TODO: move to states object
        [Header("Stats")]
        [SerializeField]
        private PromptDifficulty difficulty;
        [SerializeField] 
        private float health = 2f;
        [SerializeField]
        private float attackDamage = 1f;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField] 
        private float moveSpeed = 1f;
        
        [Header("Dependencies")]
        [SerializeField] 
        private Prompt prompt;
        [SerializeField]
        private Health healthComp;
        [SerializeField]
        private AttackCaster attackCaster;
        [SerializeField]
        private SplineMovement splineMovement;
        [SerializeField] 
        private GameObject deathFX;

        private State _state = State.Moving;
        private State _runningState;
        private State _previousState;

        public State GetState()
        {
            return _state;
        }

        public float GetHealth()
        {
            return healthComp.GetCurrentHealth();
        }

        public float GetAttackCooldown()
        {
            return attackCooldown;
        }
        
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public void OnPromptCompleted()
        {
            healthComp.TakeDamage(int.MaxValue);
        }

        public void OnHitCastle()
        {
            SetState(State.Attacking);
        }

        public void OnTakeDamage()
        {
            if (_state == State.Dead) return;
            SetState(State.Hurt);
        }
        
        public void OnHealthHitZero()
        {
            SetState(State.Dead);
        }

        public void SetPath(SplineContainer splineContainer)
        {
            splineMovement.SetPath(splineContainer);
            splineMovement.SetSpeed(moveSpeed);
            splineMovement.Move();
        }

        private void SetState(State state)
        {
            _previousState = _state;
            _state = state;
        }
        
        private void Start()
        {
            if (prompt != null) prompt.Generate(difficulty);
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
            _runningState = State.Moving;
            splineMovement.SetSpeed(moveSpeed);
            splineMovement.Move();
        }
        
        private void Idle()
        {
            _runningState = State.Idle;
            splineMovement.SetSpeed(0f);
            splineMovement.Stop();
        }

        private void Attack()
        {
            if (_runningState == State.Attacking) return;
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            _runningState = State.Attacking;
            splineMovement.Stop();
            while (_state == State.Attacking)
            {
                attackCaster.Attack(attackDamage);
                yield return new WaitForSeconds(attackCooldown);
            }
        }

        private void Hurt()
        {
            if (_runningState == State.Hurt) return;
            StartCoroutine(HurtRoutine());
        }
        
        private IEnumerator HurtRoutine()
        {
            _runningState = State.Hurt;
            splineMovement.Stop();
            yield return new WaitForSeconds(0.5f);
            SetState(_previousState);
        }
        
        private void Die()
        {
            if (_runningState == State.Dead) return;
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            _runningState = State.Dead;
            splineMovement.Stop();
            yield return new WaitForSeconds(0.5f);
            var deathEffects = Instantiate(deathFX, transform.position, transform.rotation);
            deathEffects.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            healthComp.Set(health);
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
