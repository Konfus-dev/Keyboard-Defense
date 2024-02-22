using System;
using KeyboardDefense.Prompts;
using Konfus.Systems.Sensor_Toolkit;

namespace KeyboardDefense.Characters.Enemies
{
    public class Enemy : Character
    {
        private LineScanSensor _castleSensor;
        private PromptGenerator _promptGenerator;

        private void Awake()
        {
            _castleSensor = GetComponentInChildren<LineScanSensor>();
            _promptGenerator = GetComponent<PromptGenerator>();
        }

        private void Start()
        {
            _promptGenerator.GeneratedPrompt.promptCompleted.AddListener(OnPromptCompleted);
        }

        private void FixedUpdate()
        {
            if (_castleSensor.Scan())
            {
                OnHitCastle();
            }
        }

        private void OnPromptCompleted()
        {
            Die();
        }

        private void OnHitCastle()
        {
            Attack();
        }
    }
}