using System;
using UnityEngine;

namespace Core.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Instance = this as T;
            }
        }
        protected virtual void OnApplicationQuit()
        {
            Instance = null;
        }
    }
}