using GoogleMobileAds.Api;
using System;

namespace Monetization.Ads
{
    public class CachedNativeAd
    {
        private NativeAd _nativeAd;
        public DateTime CachedTime;

        public int TimeShown;
        public NativeAd NativeAd
        {
            get
            {
                return _nativeAd;
            }
            set
            {
                _nativeAd = value;
            }
        }
        public CachedNativeAd(NativeAd nativeAd)
        {
            CachedTime = DateTime.Now;
            _nativeAd = nativeAd;
            TimeShown = 0;
        }

        public void Disolve()
        {
            _nativeAd.Destroy();
        }
    }
}