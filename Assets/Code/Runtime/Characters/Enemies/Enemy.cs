using KeyboardDefense.Prompts;
using KeyboardDefense.Services;
using Konfus.Systems.Sensor_Toolkit;

namespace KeyboardDefense.Characters.Enemies
{
    public class Enemy : Character
    {
        private IGameStateService _gameStateService;
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
            _gameStateService = ServiceProvider.Get<IGameStateService>();
            _gameStateService.GameStateChanged.AddListener(OnGameStateChanged);
            var player = ServiceProvider.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
            _promptGenerator.GeneratedPrompt.promptCompleted.AddListener(OnPromptCompleted);
        }

        private void OnGameStateChanged(IGameStateService.State state)
        {
            if (state == IGameStateService.State.Paused) SetState(State.Idle);
            if (state == IGameStateService.State.Playing) SetState(State.Moving);
        }

        private void Update()
        {
            if (_gameStateService.GameState == IGameStateService.State.Playing && _castleSensor.Scan())
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
            if (GetState() != State.Attacking)
            {
                SetState(State.Attacking);
            }
        }
        
        private void OnPlayerHealthChanged(int currHealth, int _)
        {
            if (currHealth <= 0)
            {
                _promptGenerator.GeneratedPrompt.gameObject.SetActive(false);
                SetState(State.Idle);
            }
        }
    }
}