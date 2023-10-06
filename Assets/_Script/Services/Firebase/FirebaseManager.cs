
using UnityEngine;
using Core.Singleton;
using Services.FirebaseService.Remote;
using Services.FirebaseService.Analytics;
using Services.FirebaseService.Crashlytics;
using Firebase;
using Firebase.Extensions;

namespace Services.FirebaseService
{
    public static class FirebaseManager
    {
        public static bool FirebaseInitialized { get; private set; }

        public static void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    var app = FirebaseApp.DefaultInstance;
                    FirebaseInitialized = true;
                    FirebaseCrashlytics.Init();
                    FirebaseAnalytics.Init();
                    FirebaseRemote.Init();
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + task.Result);
                }
            });
        }
    }
}
