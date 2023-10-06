
using com.adjust.sdk;
using Core.Env;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Services.Adjust
{
    public class Adjust : MonoBehaviour
    {
        public static void TrackIronsourceRevenue(IronSourceImpressionData impressionData)
        {
            if (Environment.ENV != Environment.Env.PROD)
            {
                return;
            }

            AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
            adRevenue.setRevenue((double)impressionData.revenue, "USD");
            adRevenue.setAdRevenueNetwork(impressionData.adNetwork);
            adRevenue.setAdRevenueUnit(impressionData.adUnit);
            adRevenue.setAdRevenuePlacement(impressionData.placement);
            com.adjust.sdk.Adjust.trackAdRevenue(adRevenue);
        }

        public static void TrackAdmobRevenue(AdValue adValue)
        {
            if (Environment.ENV != Environment.Env.PROD)
            {
                return;
            }
            AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            adRevenue.setRevenue(adValue.Value / 1000000f, adValue.CurrencyCode);
            com.adjust.sdk.Adjust.trackAdRevenue(adRevenue);
        }
    }
}