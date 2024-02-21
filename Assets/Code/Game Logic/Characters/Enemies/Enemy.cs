using System;
using KeyboardDefense.Prompts;
using Konfus.Systems.Sensor_Toolkit;

namespace KeyboardDefense.Characters.Enemies
{
    public class Enemy : Character
    {
        private LineScanSensor _castleSensor;
        private PromptGenerator _promptBinding;

        private void Start()
        {
            _castleSensor = GetComponentInChildren<LineScanSensor>();
            _promptBinding = GetComponent<PromptGenerator>();
        }

        private void FixedUpdate()
        {
            if (_castleSensor.Scan())
            {
                OnHitCastle();
            }
        }

        public void OnPromptCompleted()
        {
            Die();
        }

        public void OnHitCastle()
        {
            Attack();
        }
    }
}