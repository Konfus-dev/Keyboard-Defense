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
                ServiceProvider.Register<T>(Instance);
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
            // Do nothing because we don't want to destroy or unregister the singleton.
        }

        private void OnApplicationQuit()
        {
            ServiceProvider.Unregister<T>();
        }
    }
}