using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// Abstract class for all singleton components.
    /// </summary>
    /// <typeparam name="T">The type of the singleton.</typeparam>
    public abstract class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance => _instance;

        /// <summary>
        /// If true, the instances of the same type are destroyed after being instantiated.
        /// </summary>
        protected virtual bool DestroyOtherInstances => true;

        protected void Awake()
        {
            if (_instance == null)
            {
                // Ensure that the game object containing this component is a root game object.
                // Otherwise, DontDestroyOnLoad() won't work.
                transform.parent = null;

                _instance = this as T;
                DontDestroyOnLoad(gameObject);

                OnSingletonAwake();
            }
            else if (_instance != this && DestroyOtherInstances)
                Destroy(this);
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                OnSingletonDestroyed();
                _instance = null;
            }
        }

        /// <summary>
        /// Similar to Awake(), but only called by the kept instance.
        /// </summary>
        protected virtual void OnSingletonAwake() {}

        /// <summary>
        /// Similar to OnDestroy(), but only called by the kept instance.
        /// </summary>
        protected virtual void OnSingletonDestroyed() {}

        /// <summary>
        /// Create the default instance of the class if none exists.
        /// </summary>
        public static void EnsureCreation()
        {
            if (_instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                gameObject.AddComponent<T>();
            }
        }
    }
}
