using KeyboardCats.Vitality;
using UnityEngine;

namespace KeyboardCats.Projectiles
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private float damage;

        public void Shoot(float speed)
        {
            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}