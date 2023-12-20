using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.AI
{
    public class Health : MonoBehaviour
    {
        public HealthEvent healthChanged;
        public HealthEvent healthZero;
        
        [SerializeField, ReadOnly]
        private float currHealth;
        [SerializeField]
        private float maxHealth = 10;

        private void Start()
        {
            currHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currHealth -= damage;
            if (currHealth < 0) healthZero.Invoke(0, maxHealth);
            healthChanged.Invoke(currHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            currHealth += amount;
            if (currHealth > maxHealth) currHealth = maxHealth;
            healthChanged.Invoke(currHealth, maxHealth);
        }

        [Button]
        private void TestTakeDamage()
        {
            TakeDamage(1);
        }
        
        [Button]
        private void TestHeal()
        {
            Heal(1);
        }
    }
    
    /// <summary>
    /// An event triggered when health changes.
    /// the first float is current health, the second is max health.
    /// </summary>
    [Serializable]
    public class HealthEvent : UnityEvent<float, float> { }
}
