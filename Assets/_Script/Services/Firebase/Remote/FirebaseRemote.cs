
using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.RemoteConfig;
using Firebase.Extensions;

namespace Services.FirebaseService.Remote
{

    public static class FirebaseRemote
    {
        public static RemoteVariableCollection RemoteVariableCollection;

        public static Action OnFetchedCompleted;
        public static Action OnFetchedFailed;

        public static void Init()
        {
            if (RemoteVariableCollection == null)
            {
                Debug.Log("You havent provide your custom remote variable collection");
                return;
            }
            RemoteVariableCollection.AddToFetchQueue();
            FetchDataAsync();
            FirebaseLogger.Log("Initialized", FirebaseLogger.FirebaseComponent.REMOTE);
        }

        public static Task FetchDataAsync()
        {
            Task fetchTask =
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        static void FetchComplete(Task fetchTask)
        {
            FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        foreach (var kvp in RemoteVariableCollection.Variables)
                        {
                            string variableName = kvp.Key;
                            object variableValue = kvp.Value.GetValue();

                            if (variableValue.GetType() == typeof(double))
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).DoubleValue);
                            }
                            else if (variableValue.GetType() == typeof(string))
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).StringValue);
                            }
                            else if (variableValue.GetType() == typeof(bool))
                            {
                                kvp.Value.SetValue(FirebaseRemoteConfig.DefaultInstance.GetValue(variableName).BooleanValue);
                            }
                            FirebaseLogger.Log($"Feteched {variableName}: {kvp.Value.GetValue()}", FirebaseLogger.FirebaseComponent.REMOTE);
                        }
                        OnFetchedCompleted?.Invoke();
                    });
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            FirebaseLogger.Log($"Feteched Failed", FirebaseLogger.FirebaseComponent.REMOTE);
                            break;
                        case FetchFailureReason.Throttled:
                            FirebaseLogger.Log($"Fetch throttled until {info.ThrottledEndTime}", FirebaseLogger.FirebaseComponent.REMOTE);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    FirebaseLogger.Log($"Fetech pending", FirebaseLogger.FirebaseComponent.REMOTE);
                    break;
            }
        }
    }
}