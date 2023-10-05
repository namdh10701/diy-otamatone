using System;
using Core.Env;
using Firebase.Analytics;
using GoogleMobileAds.Api;

namespace Services.FirebaseService.Analytics
{
    public class FirebaseAnalytics
    {
        public static void Init()
        {
            if (Core.Env.Environment.ENV == Core.Env.Environment.Env.PROD)
            {
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Firebase.Analytics.FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
                FirebaseLogger.Log("Initialized", FirebaseLogger.FirebaseComponent.ANALYTICS);
            }
        }

        public static void LogEvent(string valueLog, params Parameter[] parameters)
        {
            if (!FirebaseManager.FirebaseInitialized)
            {
                return;
            }
            if (Core.Env.Environment.ENV != Core.Env.Environment.Env.PROD)
            {
                FirebaseLogger.Log(valueLog, FirebaseLogger.FirebaseComponent.ANALYTICS);
                return;
            }

            Firebase.Analytics.FirebaseAnalytics.LogEvent(valueLog, parameters);
        }

        public static void LogEvent(string eventName)
        {
            if (!FirebaseManager.FirebaseInitialized)
            {
                return;
            }
            FirebaseLogger.Log(eventName, FirebaseLogger.FirebaseComponent.ANALYTICS);
	    if (Core.Env.Environment.ENV != Core.Env.Environment.Env.PROD)
            {
                return;
            }
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
        }

        public static void TrackAdmobRevenue(AdValue adValue)
        {
            Parameter[] LTVParameters = {
                new Parameter("ad_platform", "adMob"),
                new Parameter("ad_source", "adMob"),
                new Parameter("value", adValue.Value / 1000000f),
                new Parameter("currency", adValue.CurrencyCode),
                new Parameter("precision", (int)adValue.Precision)
             };
            LogEvent("ad_impression", LTVParameters);
        }

        public static void TrackIronSourceRevenue(IronSourceImpressionData impressionData)
        {
            Parameter[] AdParameters = {
               new Parameter("ad_platform", "ironSource"),
                new Parameter("ad_source", impressionData.adNetwork),
                new Parameter("ad_unit_name", impressionData.adUnit),
                new Parameter("ad_format", impressionData.instanceName),
                new Parameter("currency","USD"),
                new Parameter("value", (double)impressionData.revenue)
            };
            LogEvent("ad_impression", AdParameters);
        }
    }
}
