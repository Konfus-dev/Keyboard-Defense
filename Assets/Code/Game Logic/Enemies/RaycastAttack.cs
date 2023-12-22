using KeyboardCats.Vitality;
using UnityEngine;

namespace KeyboardCats.Enemies
{
    public class RaycastAttack : MonoBehaviour
    {
        public void Attack(float attackDamage)
        {
            if (Physics.Raycast(transform.position, transform.forward * 2, out var hit))
            {
                var health = hit.transform.gameObject.GetComponent<Health>();
                health.TakeDamage(attackDamage);
            }
        }
    }
}