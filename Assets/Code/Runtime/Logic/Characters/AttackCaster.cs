using System.Linq;
using Konfus.Systems.Sensor_Toolkit;
using UnityEngine;

namespace KeyboardDefense.Logic.Characters
{
    [RequireComponent(typeof(Character))]
    public class AttackCaster : MonoBehaviour
    {
        [SerializeField]
        private LineScanSensor sensor;

        private Character _character;
        
        public void Attack()
        {
            if (!sensor.Scan()) return;
            
            var hit = sensor.hits.First();
            var health = hit.gameObject.GetComponent<IHasHealth>();
            health?.TakeDamage(_character.GetStats().AttackDamage);
        }

        private void Start()
        {
            _character = GetComponent<Character>();
        }
    }
}