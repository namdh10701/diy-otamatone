using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using System.Collections;
using Game.Shared;

namespace Monetization.IAP
{

    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private CodelessIAPButton IAPButtons;

        public void OnPurchaseCompelete(Product product)
        {
            Debug.Log("purchased success");
            PlayerPrefs.SetInt(Constant.ADS_REMOVED_KEY, 1);
            AdsController.Instance.OnRemoveAds(true);
            FirebaseAnalytics.LogEvent("REMOVES_ADS_PURCHASED");
            FirebaseAnalytics.LogEvent("REMOVES_ADS_CLICK");
            StartCoroutine(HideButton());
        }

        private IEnumerator HideButton()
        {
            yield return new WaitForSeconds(.2f);
            IAPButtons.gameObject.SetActive(false);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription description)
        {
            Debug.Log($"Purchase failed: {product}, {description}");
            FirebaseAnalytics.LogEvent("REMOVES_ADS_CLICK");
        }

        public void OnProductFetched(Product product)
        {
            if (product.receipt != null)
            {
                Debug.Log("Player has purchased removed ads");
                PlayerPrefs.SetInt(Constant.ADS_REMOVED_KEY, 1);
                IAPButtons.gameObject.SetActive(false);
                AdsController.Instance.OnRemoveAds(true);
            }

        }
    }
}