using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.Vitality
{
    public class Health : MonoBehaviour
    {
        public HealthEvent healthChanged;
        public HealthEvent healthZero;
        
        [SerializeField, ReadOnly]
        private float currHealth;
        [SerializeField]
        private float maxHealth = 10;

        public float GetCurrentHealth()
        {
            return currHealth;
        }
        
        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public void Set(float value)
        {
            currHealth = value;
            maxHealth = value;
        }
        
        public void TakeDamage(float damage)
        {
            currHealth -= damage;
            if (currHealth <= 0) healthZero.Invoke(0, maxHealth);
            healthChanged.Invoke(currHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            currHealth += amount;
            if (currHealth > maxHealth) currHealth = maxHealth;
            healthChanged.Invoke(currHealth, maxHealth);
        }
        
        private void Start()
        {
            currHealth = maxHealth;
        }

        private void OnValidate()
        {
            currHealth = maxHealth;
        }
    }
    
    /// <summary>
    /// An event triggered when health changes.
    /// the first float is current health, the second is max health.
    /// </summary>
    [Serializable]
    public class HealthEvent : UnityEvent<float, float> { }
}
