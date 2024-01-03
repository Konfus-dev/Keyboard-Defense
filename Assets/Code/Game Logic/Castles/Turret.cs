using KeyboardCats.Projectiles;
using Konfus.Utility.Extensions;
using UnityEngine;

namespace KeyboardCats.Castles
{
    public class Turret : MonoBehaviour
    {
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float range = 8;
        [SerializeField] private float aimSpeed = 8;
        [SerializeField] private float fireCooldown = 1;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform shootPos;
        [SerializeField] private Animator animator;

        private Transform _target;
        private float _cooldownTimer;

        private void Start()
        {
            _cooldownTimer = fireCooldown;
        }

        private void Update()
        {
            FindTarget();

            if (_target != null)
            {
                Rotate();
                Fire();
            }
        }

        private void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            foreach (Collider col in colliders)
            {
                if (!targetMask.Contains(col.gameObject.layer)) continue;
                
                if (_target == null)
                {
                    // Set the first found enemy as the current target
                    _target = col.transform;
                }
                else
                {
                    // Check if the new enemy is closer than the current target
                    float distanceToNewTarget = Vector3.Distance(transform.position, col.transform.position);
                    float distanceToCurrentTarget = Vector3.Distance(transform.position, _target.position);

                    if (distanceToNewTarget < distanceToCurrentTarget)
                    {
                        _target = col.transform;
                    }
                }
            }
        }

        private void Rotate()
        {
            Vector3 targetDirection = _target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
        }

        private void Fire()
        {
            // If the cooldown timer is greater than zero, decrement it
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.fixedDeltaTime;
                animator.SetBool(IsShooting, false);
            }
            else
            {
                // Instantiate projectile and set its direction towards the target
                GameObject loadedProjectile = Instantiate(projectile, shootPos.position, shootPos.rotation);
                loadedProjectile.GetComponent<Projectile>().Shoot(10);
                animator.SetBool(IsShooting, true);
                _cooldownTimer = fireCooldown;
            }
        }
    }
}
