using System.Linq;
using KeyboardCats.Vitality;
using Konfus.Systems.Sensor_Toolkit;
using UnityEngine;

namespace KeyboardCats.Enemies
{
    public class AttackCaster : MonoBehaviour
    {
        [SerializeField]
        private LineScanSensor sensor;
        
        public void Attack(float attackDamage)
        {
            if (sensor.Scan())
            {
                var hit = sensor.hits.First();
                Debug.Log("Attacking: " + hit.gameObject.name);
                var health = hit.gameObject.GetComponent<Health>();
                if (health != null) health.TakeDamage(attackDamage);
            }
        }
    }
}