using System;
using KeyboardDefense.Prompts;
using KeyboardDefense.Services;
using Konfus.Systems.Sensor_Toolkit;
using UnityEngine;

namespace KeyboardDefense.Characters.Enemies
{
    public class Enemy : Character
    {
        private LineScanSensor _castleSensor;
        private PromptGenerator _promptGenerator;
        
        protected override void OnSpawn()
        {
            base.OnSpawn();
            SetState(State.Moving);
        }
        
        private void Awake()
        {
            _castleSensor = GetComponentInChildren<LineScanSensor>();
            _promptGenerator = GetComponent<PromptGenerator>();
        }

        // TODO: maybe follow pattern of other things such as AddToScoreOnDeath and PlayEffectOnDeath and create a KillOnPromptCompleted script
        private void Start()
        {
            var player = ServiceProvider.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
            _promptGenerator.GeneratedPrompt.promptCompleted.AddListener(OnPromptCompleted);
        }

        private void Update()
        {
            if (_castleSensor.Scan())
            {
                OnHitCastle();
            }
        }

        private void OnPromptCompleted()
        {
            TakeDamage(GetStats().Health);
        }

        private void OnHitCastle()
        {
            SetState(State.Attacking);
        }
        
        private void OnPlayerHealthChanged(int currHealth, int _)
        {
            if (currHealth <= 0)
            {
                TakeDamage(Int32.MaxValue);
            }
        }
    }
}