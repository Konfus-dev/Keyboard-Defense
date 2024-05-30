namespace KeyboardDefense.Services
{
    public abstract class SingletonGameService<T> : GameService where T : class, IGameService
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance { get; private set; } = null;
        
        public override void Register()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                ServiceProvider.Register<T>(this);
            }
            else if (!ReferenceEquals(Instance, this))
            {
                Destroy(gameObject);
            }
        }
        
        public override void Unregister()
        {
            Instance = null;
            Destroy(gameObject);
            ServiceProvider.Unregister<T>();
        }

        protected override void OnDestroy()
        {
            // Do nothing on destroy because we don't want to destroy the singleton or unregister it
        }

        private void OnApplicationQuit()
        {
            Unregister();
        }
    }
}