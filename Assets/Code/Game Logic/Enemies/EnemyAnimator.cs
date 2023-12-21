using UnityEngine;

namespace KeyboardCats.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Enemy enemy;
        
        private void Update()
        {
            switch (enemy.GetState())
            {
                case Enemy.State.Idle:
                {
                    animator.Play("Idle_A");
                    animator.Play("Eyes_Blink");
                    break;
                }
                case Enemy.State.Moving:
                {
                    switch (enemy.GetMoveSpeed())
                    {
                        case > 0.5f:
                            animator.Play("Run");
                            break;
                        default:
                            animator.Play("Walk");
                            break;
                    }

                    animator.Play("Eyes_Blink");
                    break;
                }
                case Enemy.State.Attacking:
                {
                    animator.Play("Attack");
                    animator.Play("Eyes_Annoyed");
                    break;
                }
                case Enemy.State.Hurt:
                {
                    animator.Play("Eyes_Cry");
                    animator.Play("Hit");
                    break;
                }
                case Enemy.State.Dead:
                {
                    animator.Play("Death");
                    animator.Play("Eyes_Dead");
                    break;
                }
            }
        }
    }
}