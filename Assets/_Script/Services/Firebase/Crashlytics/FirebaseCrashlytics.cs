using Firebase;
using UnityEngine;
using Core.Env;
namespace Services.FirebaseService.Crashlytics
{
    public static class FirebaseCrashlytics
    {
        public static void Init()
        {
            if (Environment.ENV == Environment.Env.PROD)
            {
                Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                FirebaseLogger.Log("Initialized", FirebaseLogger.FirebaseComponent.CRASHLYTICS);
            }
        }
    }
}
