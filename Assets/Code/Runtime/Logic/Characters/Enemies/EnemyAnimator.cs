using System.Collections;
using UnityEngine;

namespace KeyboardDefense.Logic.Characters.Enemies
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(SplineMovement))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        private Enemy _enemy;
        private SplineMovement _splineMovement;
        private Character.State _playingState;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _splineMovement = GetComponent<SplineMovement>();
        }

        private void Update()
        {
            if (animator == null) return;
            
            switch (_enemy.GetState())
            {
                case Character.State.Idle when _playingState != Character.State.Idle:
                {
                    PlayIdleAnim();
                    break;
                }
                case Character.State.Moving when _playingState != Character.State.Moving:
                {
                    PlayMoveAnim();
                    break;
                }
                case Character.State.Attacking when _playingState != Character.State.Attacking:
                {
                    PlayAttackAnim();
                    break;
                }
                case Character.State.Hurt when _playingState != Character.State.Hurt:
                {
                    PlayHurtAnim();
                    break;
                }
                case Character.State.Dead when _playingState != Character.State.Dead:
                {
                    PlayDeathAnim();
                    break;
                }
            }
        }

        private void PlayIdleAnim()
        {
            _playingState = Character.State.Idle;
            animator.Play("Idle_A");
            animator.Play("Eyes_Blink");
        }

        private void PlayMoveAnim()
        {
            _playingState = Character.State.Moving;
            switch (_splineMovement.GetCurrentSpeed())
            {
                case > 2f:
                    animator.Play("Run");
                    break;
                default:
                    animator.Play("Walk");
                    break;
            }

            animator.Play("Eyes_Blink");
        }
        
        private void PlayHurtAnim()
        {
            _playingState = Character.State.Hurt;
            animator.Play("Eyes_Cry");
            animator.Play("Hit");
        }
        
        private void PlayDeathAnim()
        {
            _playingState = Character.State.Dead;
            animator.Play("Death");
            animator.Play("Eyes_Dead");
        }

        private void PlayAttackAnim()
        {
            _playingState = Character.State.Attacking;
            StartCoroutine(PlayAttackAnimRoutine());
        }
        
        private IEnumerator PlayAttackAnimRoutine()
        {
            while (_playingState == Character.State.Attacking)
            {
                animator.Play("Attack");
                animator.Play("Eyes_Annoyed");
                yield return new WaitForSeconds(0.2f);
                animator.Play("Idle_A");
                yield return new WaitForSeconds(_enemy.GetStats().AttackCooldown - 0.2f);
            }
        }
    }
}