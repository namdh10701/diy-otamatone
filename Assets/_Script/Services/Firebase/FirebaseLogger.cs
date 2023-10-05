using UnityEngine;
public static class FirebaseLogger
{
    public enum FirebaseComponent
    {
        ANALYTICS, REMOTE, CRASHLYTICS
    }
    public static bool IsDebugAnalytics;
    public static bool IsDebugRemote;
    public static bool IsDebugCrashlytics;

    public static void Log(string logDetails, FirebaseComponent component)
    {
        switch (component)
        {
            case FirebaseComponent.ANALYTICS:
                Debug.Log($"<color=cyan>ANALYTICS: {logDetails}</color>");
                break;
            case FirebaseComponent.REMOTE:
                Debug.Log($"<color=pink>REMOTE: {logDetails}</color>");
                break;
            case FirebaseComponent.CRASHLYTICS:
                Debug.Log($"<color=red>CRASHLYTICS: {logDetails}</color>");
                break;
        }
    }
}