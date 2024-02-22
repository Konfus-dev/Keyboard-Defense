using KeyboardDefense.Logic.Prompts;
using Konfus.Systems.Sensor_Toolkit;

namespace KeyboardDefense.Logic.Characters.Enemies
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

        protected override void OnSpawn()
        {
            base.OnSpawn();
            SetState(State.Moving);
        }

        protected override void OnUpdate()
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
    }
}