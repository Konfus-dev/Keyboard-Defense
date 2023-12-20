using System.Collections;
using Konfus.Utility.Extensions;
using UnityEngine;

namespace KeyboardCats.Enemies
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float attackDamage = 1f;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField] 
        private float moveSpeed = 0.1f;
        [SerializeField]
        private Health health;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private GameObject visual;

        private State _state = State.Moving;
        private Transform _target;

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

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                {
                    moveSpeed = 0;
                    animator.Play("Idle_A");
                    animator.Play("Eyes_Blink");
                    break;
                }
                case State.Moving:
                {
                    switch (moveSpeed)
                    {
                        case > 0.5f:
                            animator.Play("Run");
                            break;
                        default:
                            animator.Play("Walk");
                            break;
                    }

                    animator.Play("Eyes_Blink");
                    transform.MoveTo(_target.position, moveSpeed * Time.fixedDeltaTime);
                    break;
                }
                case State.Attacking:
                {
                    moveSpeed = 0;
                    animator.Play("Attack");
                    animator.Play("Eyes_Annoyed");
                    break;
                }
                case State.Hurt:
                {
                    animator.Play("Eyes_Cry");
                    animator.Play("Hit");
                    break;
                }
                case State.Dead:
                {
                    moveSpeed = 0;
                    animator.Play("Death");
                    animator.Play("Eyes_Dead");
                    break;
                }
            }
        }
        
        private void Attack()
        {
            moveSpeed = 0;
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            while (_state == State.Attacking)
            {
                _state = State.Idle;
                yield return new WaitForSeconds(attackCooldown);
                _state = State.Attacking;
                if (Physics.Raycast(transform.position, transform.forward * 2, out var hit))
                {
                    var health = hit.transform.gameObject.GetComponent<Health>();
                    health.TakeDamage(attackDamage);
                }
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
            var oldMoveSpeed = moveSpeed;
            moveSpeed = 0;
            yield return new WaitForSeconds(0.5f);
            moveSpeed = oldMoveSpeed;
        }
        
        private void Die()
        {
            _state = State.Dead;
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }

        private enum State
        {
            Idle,
            Moving,
            Attacking,
            Hurt,
            Dead
        }
    }
}
