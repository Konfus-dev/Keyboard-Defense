namespace KeyboardDefense.Services
{
    public abstract class SingletonGameService<T> : GameService where T : class
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance { get; private set; }
        
        public override void Register()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                ServiceProvider.Instance.Register<T>(this);
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
            ServiceProvider.Instance.Unregister<T>();
        }
    }
}