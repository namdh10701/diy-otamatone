using Game.Craft;
using Game.RemoteVariable;
using Game.Shared;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Monetization.Ads;
using UnityEngine;

public class AdsHandler : MonoBehaviour
{
    [SerializeField] private bool _isDebugNative;
    [SerializeField] private bool _isDebugBanner;
    [SerializeField] private bool _isDebugOpen;
    [SerializeField] private bool _isDebugRewarded;
    [SerializeField] private bool _isDebugInter;

    private void Awake()
    {
        AdsLogger.IsDebugBanner = _isDebugBanner;
        AdsLogger.IsDebugRewarded = _isDebugRewarded;
        AdsLogger.IsDebugInter = _isDebugInter;
        AdsLogger.IsDebugNative = _isDebugNative;
        AdsLogger.IsDebugOpen = _isDebugOpen;
    }
    public static bool AdRemoved()
    {
        return PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) == 1;
    }

    private void Start()
    {
        ApplyRemoteConfig();


        AdsController.Instance.OnRemoveAds(AdRemoved());
        AdsController.Instance.Init();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    public void LoadBannerAndNativeAd()
    {
        if (PlayerPrefs.GetInt(Constant.ADS_REMOVED_KEY) != 1)
        {
            InvokeRepeating("TurnBannerOn", 0, 5);
            InvokeRepeating("LoadNativeAd", 30, 30);
        }
    }

    public void ApplyRemoteConfig()
    {
        RemoteVariable remoteVariable = RemoteVariable.Convert(RemoteVariableManager.Instance.MyRemoteVariables);

        AdsController.Instance.SetInterOn(remoteVariable.IsInterOn);
        AdsController.Instance.SetRewardOn(remoteVariable.IsRewardOn);
        AdsIntervalValidator.INTERVAL_REWARD_INTER = remoteVariable.InterRewardInterval;
        AdsIntervalValidator.INTERVAL_OPEN_OPEN = remoteVariable.OpenOpenInterval;
        AdsIntervalValidator.INTERVAL_OPEN_INTER = remoteVariable.OpenInterInterval;
        AdsIntervalValidator.INTERVAL_INTER_INTER = remoteVariable.InterInterInterval;


        CraftSequenceManager.BgTapShowInter = remoteVariable.BgTapShowInter;
        CraftSequenceManager.BodyTapShowInter = remoteVariable.BodyTapShowInter;
        CraftSequenceManager.HeadTapShowInter = remoteVariable.HeadTapShowInter;
        CraftSequenceManager.EyeTapShowInter = remoteVariable.EyeTapShowInter;
        CraftSequenceManager.MonsterTapShowInter = remoteVariable.MonsterTapShowInter;
        CraftSequenceManager.MouthTapShowInter = remoteVariable.MouthTapShowInter;
        CraftSequenceManager.SequenceBtnTapShowInter = remoteVariable.SequenceBtnTapShowInter;
        CraftSequenceManager.BeforeWinPanelShowInter = remoteVariable.BeforeWinPanelShowInter;
        CraftSequenceManager.NextBtnTapShowInter = remoteVariable.NextBtnTapShowInter;
        
        //TODO AdInterval
    }

    private void EndFreeInterTime()
    {
        AdsController.Instance._isFreeAdsTimeEnded = true;
    }

    private void TurnBannerOn()
    {
        AdsController.Instance.ShowBanner();
    }
    private void LoadNativeAd()
    {
        AdsController.Instance.LoadNativeAds();
    }
    public void OnAppStateChanged(AppState state)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                AdsController.Instance.ShowAppOpenAd();
            }
        });
    }
}