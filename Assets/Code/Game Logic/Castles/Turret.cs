using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeyboardCats.Enemies;
using KeyboardCats.Projectiles;
using UnityEngine;

namespace KeyboardCats.Castles
{
    [RequireComponent(typeof(SphereCollider))]
    public class Turret : MonoBehaviour
    {
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        
        [SerializeField] 
        private float range = 8;
        [SerializeField] 
        private float shootCooldown = 1;
        [SerializeField]
        private Transform shootPos;
        [SerializeField]
        private GameObject projectile;
        [SerializeField] 
        private Animator animator;

        private Projectile _loadedProjectile;
        private Queue<Enemy> _enemiesInRange;
        private Transform _target;
        private bool _shooting;

        private void Start()
        {
            _enemiesInRange = new Queue<Enemy>();
            Reload();
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) _enemiesInRange.Enqueue(enemy);
        }

        private void Update()
        {
            // Aim
            if (_target) transform.LookAt(_target);
            else transform.LookAt(transform.parent.forward);
            // Shoot
            if (_enemiesInRange.Any()) ShootAtEnemiesInRange();
        }

        private void ShootAtEnemiesInRange()
        {
            var nearestEnemy = _enemiesInRange.Peek();
            if (nearestEnemy != null && (transform.position - nearestEnemy.transform.position).magnitude <= range && !_shooting)
            { 
                var enemyToShootAt = _enemiesInRange.Dequeue();
                StartCoroutine(ShootAtNearestEnemyRoutine(enemyToShootAt));
            }
        }

        private IEnumerator ShootAtNearestEnemyRoutine(Enemy enemyToShootAt)
        {
            _shooting = true;
            _target = enemyToShootAt.transform;
            
            while (_target)
            {
                yield return new WaitForSeconds(shootCooldown/2);
                Reload();
                yield return new WaitForSeconds(shootCooldown/2);
                Shoot();
            }

            _target = null;
            _shooting = false;
        }

        private void Reload()
        {
            if (_loadedProjectile != null) return;
        
            animator.SetBool(IsShooting, false);
            GameObject loadedProjectile = Instantiate(projectile, shootPos);
            _loadedProjectile = loadedProjectile.GetComponent<Projectile>();
        }

        private void Shoot()
        {
            if (_loadedProjectile == null) return;
            animator.SetBool(IsShooting, true);
            _loadedProjectile.GetComponent<Rigidbody>().isKinematic = false;
            _loadedProjectile.GetComponent<Projectile>().Shoot(range);
            _loadedProjectile = null;
        }
    }
}
