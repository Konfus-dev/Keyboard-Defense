using System.Collections;
using KeyboardCats.Vitality;
using Konfus.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.Projectiles
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public UnityEvent hitSomething;

        [Header("Settings")]
        [SerializeField]
        private float damage;
        [SerializeField] 
        private float lifeSpanAfterHit = 0.2f;
        [SerializeField] 
        private LayerMask hitMask;
        
        [Header("Dependencies")]
        [SerializeField]
        private Rigidbody rb;

        private bool _hasHit;

        public void Shoot(float speed)
        {
            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (_hasHit || !hitMask.Contains(collision.gameObject.layer)) return;
            
            _hasHit = true;
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
            hitSomething.Invoke();
            StartCoroutine(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(lifeSpanAfterHit);
            Destroy(gameObject);
        }
    }
}