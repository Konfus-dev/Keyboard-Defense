using System.Collections;
using UnityEngine;

namespace KeyboardCats.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Enemy enemy;

        private Enemy.State _playingState;

        private void Update()
        {
            switch (enemy.GetState())
            {
                case Enemy.State.Idle when _playingState != Enemy.State.Idle:
                {
                    PlayIdleAnim();
                    break;
                }
                case Enemy.State.Moving when _playingState != Enemy.State.Moving:
                {
                    PlayMoveAnim();
                    break;
                }
                case Enemy.State.Attacking when _playingState != Enemy.State.Attacking:
                {
                    PlayAttackAnim();
                    break;
                }
                case Enemy.State.Hurt when _playingState != Enemy.State.Hurt:
                {
                    PlayHurtAnim();
                    break;
                }
                case Enemy.State.Dead when _playingState != Enemy.State.Dead:
                {
                    PlayDeathAnim();
                    break;
                }
            }
        }

        private void PlayIdleAnim()
        {
            _playingState = Enemy.State.Idle;
            animator.Play("Idle_A");
            animator.Play("Eyes_Blink");
        }

        private void PlayMoveAnim()
        {
            _playingState = Enemy.State.Moving;
            switch (enemy.GetMoveSpeed())
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
            _playingState = Enemy.State.Hurt;
            animator.Play("Eyes_Cry");
            animator.Play("Hit");
        }
        
        private void PlayDeathAnim()
        {
            _playingState = Enemy.State.Dead;
            animator.Play("Death");
            animator.Play("Eyes_Dead");
        }

        private void PlayAttackAnim()
        {
            _playingState = Enemy.State.Attacking;
            StartCoroutine(PlayAttackAnimRoutine());
        }
        
        private IEnumerator PlayAttackAnimRoutine()
        {
            while (_playingState == Enemy.State.Attacking)
            {
                animator.Play("Attack");
                animator.Play("Eyes_Annoyed");
                yield return new WaitForSeconds(0.2f);
                animator.Play("Idle_A");
                yield return new WaitForSeconds(enemy.GetAttackCooldown() - 0.2f);
            }
        }
    }
}