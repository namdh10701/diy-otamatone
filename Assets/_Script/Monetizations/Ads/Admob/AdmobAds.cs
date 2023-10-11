using GoogleMobileAds.Api;
using Services.FirebaseService.Analytics;
using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Common;
using Core.Env;
using Game.Shared;
using Services.Adjust;
using static Monetization.Ads.AdsController;

namespace Monetization.Ads
{
    public enum NativeAdsKeyId
    {
        Key1, Key2
    }
    public class NativeAdsKey
    {
        public NativeAdsKeyId Id;
        public string Value;
        public bool IsRequesting;
        public int AttemptCount;
        public NativeAdsKey(NativeAdsKeyId id, string value)
        {
            this.Id = id;
            this.Value = value;
            this.IsRequesting = false;
            this.AttemptCount = 0;
        }
    }
    public class AdmobAds : MonoBehaviour
    {
        private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
        private DateTime appOpenExpireTime;

        private AppOpenAd appOpenAd;

        private bool _initilized = false;

        [SerializeField] private string _appOpenAdId = "";
        [SerializeField] private string _nativeAdId1 = "";
        [SerializeField] private string _nativeAdId2 = "";

        private int _openAdRequestTry;
        private int _nativeAdId1RequestTry;
        private int _nativeAdId2RequestTry;

        private NativeAdsKey _nativeAdKey1;
        private NativeAdsKey _nativeAdKey2;
        public void Init()
        {

            if (Core.Env.Environment.ENV != Core.Env.Environment.Env.PROD)
            {
                string appOpenAdTestId = "ca-app-pub-3940256099942544/3419835294";
                string nativeAdTestId = "ca-app-pub-3940256099942544/2247696110";
                _nativeAdId1 = nativeAdTestId;
                _nativeAdId2 = nativeAdTestId;
                _appOpenAdId = appOpenAdTestId;
                _openAdRequestTry = 0;
                _nativeAdId1RequestTry = 0;
                _nativeAdId2RequestTry = 0;
            }
            _nativeAdKey1 = new NativeAdsKey(NativeAdsKeyId.Key1, _nativeAdId1);
            _nativeAdKey2 = new NativeAdsKey(NativeAdsKeyId.Key2, _nativeAdId2);
            MobileAds.Initialize(HandleInitCompleteAction);
        }
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            _initilized = true;
            AdsLogger.Log($"Admob initialized", AdsController.AdType.OPEN);
            LoadAppOpenAd();
            LoadNativeAds();
        }
        void HandleAdPaid(AppOpenAd ad)
        {
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Adjust.TrackAdmobRevenue(adValue);
                FirebaseAnalytics.TrackAdmobRevenue(adValue);
            };
        }
        #region APPOPEN
        private bool _isRequestingAppOpenAd = false;
        public bool IsAppOpenAdAvailable
        {
            get
            {
                bool ret = appOpenAd != null
                        && appOpenAd.CanShowAd()
                        && DateTime.Now < appOpenExpireTime;
                if (!ret)
                {
                    LoadAppOpenAd();
                }
                return ret;
            }
        }

        public void LoadAppOpenAd()
        {
            AdsLogger.Log($"load open ad", AdsController.AdType.OPEN);
            if (!_initilized || !AdsController.Instance.HasInternet)
            {
                AdsLogger.Log($"Error: not has internet", AdsController.AdType.OPEN);
                return;
            }
            if (_isRequestingAppOpenAd)
            {
                return;
            }
            if (appOpenAd != null)
            {
                return;
            }
            AdsLogger.Log($"Requesting", AdsController.AdType.OPEN);
            FirebaseAnalytics.LogEvent(Constant.AD_REQUEST);
            _isRequestingAppOpenAd = true;
            _openAdRequestTry++;
            AppOpenAd.Load(_appOpenAdId, new AdRequest(),
                (AppOpenAd ad, LoadAdError loadError) => HandleLoadedAppOpenAd(ad, loadError)
            );

            void HandleLoadedAppOpenAd(AppOpenAd ad, LoadAdError loadError)
            {
                _isRequestingAppOpenAd = false;
                if (loadError != null)
                {
                    AdsLogger.Log($"Error: {loadError.ToString()}", AdsController.AdType.OPEN);

                }
                appOpenAd = ad;
                FirebaseAnalytics.LogEvent(Constant.AD_REQUEST_SUCCEED);
                appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;
                _openAdRequestTry = 0;
                AdsLogger.Log($"Loaded", AdsController.AdType.OPEN);
                HandleOpenAdOpened(ad);
                HandleOpenAdClosed(ad);
                HandleOpenAdShowFailed(ad);
                HandleAdPaid(ad);
                void HandleOpenAdOpened(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentOpened += () =>
                    {
                        AdsController.Instance.IsShowingOpenAd = true;
                        FirebaseAnalytics.LogEvent("ad_open_show");
                    };
                }
                void HandleOpenAdClosed(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            AdsIntervalValidator.SetInterval(AdsController.AdType.OPEN);
                            DestroyAppOpenAd();
                            LoadAppOpenAd();
                            AdsLogger.Log("On Close", AdType.OPEN);
                            AdsController.Instance.IsShowingOpenAd = false;
                        });

                    };
                }
                void HandleOpenAdShowFailed(AppOpenAd ad)
                {
                    ad.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        AdsController.Instance.IsShowingOpenAd = false;
                        DestroyAppOpenAd();
                        LoadAppOpenAd();
                        AdsLogger.Log("Error show failed: " + error.ToString(), AdType.OPEN);
                    };
                }
            }
        }
        void DestroyAppOpenAd()
        {
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }
        }
        public void ShowAppOpenAd()
        {
            StartCoroutine(PassframeShowAd());
        }

        public IEnumerator PassframeShowAd()
        {
            yield return new WaitForSeconds(.1f);
            if (AdsController.Instance.RewardedAdJustClose)
            {
                AdsLogger.Log("Go in here reward just closed", AdType.OPEN);
                AdsController.Instance.RewardedAdJustClose = false;
                yield break;
            }
            if (AdsController.Instance.IsShowingInterAd || AdsController.Instance.IsShowingOpenAd)
            {
                AdsLogger.Log("Go in here is showing open or inter", AdType.OPEN);
                yield break;
            }
            AdsLogger.Log("All conditions pass \n Show", AdsController.AdType.OPEN);
            appOpenAd.Show();
        }

        #endregion
        #region Native
        private bool _isNativeAdKey1Requesting = false;
        private bool _isNativeAdKey2Requesting = false;

        public void LoadNativeAds()
        {
            if (!_initilized)
            {
                return;
            }

            if (!_isNativeAdKey1Requesting)
            {
                _isNativeAdKey1Requesting = true;
                _nativeAdId1RequestTry++;
                FirebaseAnalytics.LogEvent(Constant.AD_REQUEST);
                FirebaseAnalytics.LogEvent("ad_native_load");
                AdLoader adLoader1 = new AdLoader.Builder(_nativeAdId1).ForNativeAd().Build();
                adLoader1.OnNativeAdLoaded += (sender, e) => HandleNativeAdLoaded(sender, e, _nativeAdId1);
                adLoader1.OnAdFailedToLoad += (sender, e) => HandleAdFailedToLoad(sender, e, _nativeAdId1);
                adLoader1.OnNativeAdClicked += (sender, e) =>
                {
                    FirebaseAnalytics.LogEvent(Constant.CLICK_NATIVE_AD);
                };
                AdsLogger.Log($"Requesting using key 1", AdsController.AdType.NATIVE);
                adLoader1.LoadAd(new AdRequest());
            }
            if (!_isNativeAdKey2Requesting)
            {
                _isNativeAdKey2Requesting = true;
                _nativeAdId2RequestTry++;
                FirebaseAnalytics.LogEvent(Constant.AD_REQUEST);
                FirebaseAnalytics.LogEvent("ad_native_load");
                AdLoader adLoader2 = new AdLoader.Builder(_nativeAdId2).ForNativeAd().Build();
                adLoader2.OnNativeAdLoaded += (sender, e) => HandleNativeAdLoaded(sender, e, _nativeAdId2);
                adLoader2.OnAdFailedToLoad += (sender, e) => HandleAdFailedToLoad(sender, e, _nativeAdId2);
                adLoader2.OnNativeAdClicked += (sender, e) =>
                {
                    FirebaseAnalytics.LogEvent(Constant.CLICK_NATIVE_AD);
                };
                AdsLogger.Log($"Requesting using key 2", AdsController.AdType.NATIVE);
                adLoader2.LoadAd(new AdRequest());
            }
            /*if (!_initilized)
            {
                return;
            }
            LoadNativeAds(NativeAdsKeyId.Key1);
            LoadNativeAds(NativeAdsKeyId.Key2);*/
        }

        /*private void LoadNativeAds(NativeAdsKeyId keyId)
        {
            if (!_initilized)
            {
                return;
            }
            NativeAdsKey key;
            if (keyId == NativeAdsKeyId.Key1)
            {
                key = _nativeAdKey1;
            }
            else
            {
                key = _nativeAdKey2;
            }

            if (key.IsRequesting)
            {
                return;
            }
            key.AttemptCount++;
            FirebaseAnalytics.LogEvent(Constant.AD_REQUEST);
            FirebaseAnalytics.LogEvent("ad_native_load");
            AdLoader adLoader = new AdLoader.Builder(key.Value).ForNativeAd().Build();
            adLoader.OnNativeAdLoaded += (sender, e) => HandleNativeAdLoaded(sender, e, key);
            adLoader.OnAdFailedToLoad += (sender, e) => HandleAdFailedToLoad(sender, e, key);
            adLoader.OnNativeAdClicked += (sender, e) =>
            {
                FirebaseAnalytics.LogEvent(Constant.CLICK_NATIVE_AD);
            };
            AdsLogger.Log($"Requesting using key {key.Id}", AdsController.AdType.NATIVE);
            adLoader.LoadAd(new AdRequest());
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e, NativeAdsKey key)
        {
            AdsLogger.Log($"Load failed {e.LoadAdError} attempt {key.AttemptCount}", AdsController.AdType.NATIVE);
            key.IsRequesting = false;
            if (key.AttemptCount >= 3)
            {
                key.AttemptCount = 0;
                return;
            }
            LoadNativeAds();
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e, NativeAdsKey key)
        {
            AdsLogger.Log($"Loaded", AdsController.AdType.NATIVE);
            FirebaseAnalytics.LogEvent(Constant.AD_REQUEST_SUCCEED);
            AdsController.Instance.OnNativeAdLoaded(e.nativeAd);
            e.nativeAd.OnPaidEvent += NativeAd_OnPaidEvent;
            key.IsRequesting = false;
        }*/


        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e, string key)
        {
            if (key == _nativeAdId1)
            {
                _isNativeAdKey1Requesting = false;
            }
            else
                _isNativeAdKey2Requesting = false;
            AdsLogger.Log($"Load failed {e.LoadAdError}", AdsController.AdType.NATIVE);
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e, string key)
        {
            AdsLogger.Log($"Loaded", AdsController.AdType.NATIVE);
            FirebaseAnalytics.LogEvent(Constant.AD_REQUEST_SUCCEED);
            AdsController.Instance.OnNativeAdLoaded(e.nativeAd);
            e.nativeAd.OnPaidEvent += NativeAd_OnPaidEvent;
            if (key == _nativeAdId1)
                _isNativeAdKey1Requesting = false;
            else
                _isNativeAdKey2Requesting = false;
        }

        private void NativeAd_OnPaidEvent(object sender, AdValueEventArgs e)
        {
            AdsLogger.Log($"Native ad paid", AdsController.AdType.NATIVE);
            Adjust.TrackAdmobRevenue(e.AdValue);
            FirebaseAnalytics.TrackAdmobRevenue(e.AdValue);
        }
        #endregion

    }
}