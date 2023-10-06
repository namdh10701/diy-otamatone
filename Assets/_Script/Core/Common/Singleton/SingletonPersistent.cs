using UnityEngine;

namespace Core.Singleton
{
    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}