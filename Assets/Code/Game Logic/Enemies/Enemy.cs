using System.Collections;
using KeyboardCats.Prompts;
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
            //FindOpenSpace();
            Attack();
        }

        private void FindOpenSpace()
        {
            if (Physics.CheckSphere(transform.position, 0.1f, LayerMask.GetMask("Enemy")))
            {
                var currPos = transform.position;
                transform.position = new Vector3(currPos.x + Random.Range(-0.5f, 0.5f), currPos.y, currPos.z);
            }
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
            _state = State.Attacking;
            splineMovement.Stop();
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            while (_state == State.Attacking)
            {
                _state = State.Attacking;
                raycastAttack.Attack(attackDamage);
                yield return new WaitForSeconds(0.2f);
                _state = State.Idle;
                yield return new WaitForSeconds(attackCooldown);
                _state = State.Attacking;
            }
        }

        private void Hurt()
        {
            if (_state == State.Dead) return;
            _state = State.Hurt;
            StartCoroutine(HurtRoutine());
        }
        
        private IEnumerator HurtRoutine()
        {
            var previousState = _state;
            splineMovement.Stop();
            yield return new WaitForSeconds(0.5f);
            _state = previousState;
            splineMovement.Move();
        }
        
        private void Die()
        {
            _state = State.Dead;
            splineMovement.Stop();
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
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
