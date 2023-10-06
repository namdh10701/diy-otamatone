using Game.RemoteVariable;
using Services.FirebaseService;
using Services.FirebaseService.Remote;
using UnityEngine;

public class FirebaseHandler : MonoBehaviour
{
    [SerializeField] private bool _isDebugAnalytics;
    [SerializeField] private bool _isDebugCrashlytics;
    [SerializeField] private bool _isDebugRemote;
    [SerializeField] private AdsHandler _adsHandler;
    private void Awake()
    {
        FirebaseLogger.IsDebugAnalytics = _isDebugAnalytics;
        FirebaseLogger.IsDebugCrashlytics = _isDebugCrashlytics;
        FirebaseLogger.IsDebugRemote = _isDebugRemote;
        Init();
    }
    public void Init()
    {
        FirebaseRemote.RemoteVariableCollection = RemoteVariableManager.Instance.MyRemoteVariables;
        FirebaseManager.Init();
        FirebaseRemote.OnFetchedCompleted += () => SaveRemoteVariable();
    }
    private void SaveRemoteVariable()
    {
        RemoteVariableManager.Instance.SaveDatas();
        _adsHandler.ApplyRemoteConfig();
    }
}