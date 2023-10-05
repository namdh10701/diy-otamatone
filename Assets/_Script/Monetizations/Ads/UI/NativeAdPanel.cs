using Game.Shared;
using GoogleMobileAds.Api;
using Services.FirebaseService.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Monetization.Ads.UI
{
    public class NativeAdPanel : MonoBehaviour
    {
        private CachedNativeAd _cachedNativeAd;
        private NativeAd _nativeAd;
        [SerializeField] private int _id;
        [SerializeField] private GameObject loadingObject;
        [SerializeField] private GameObject loadedObject;
        [SerializeField] private RawImage icon;
        [SerializeField] private RawImage image;
        [SerializeField] private RawImage adChoices;
        [SerializeField] private Image rateStar;
        [SerializeField] private Text headline;
        [SerializeField] private Text body;
        [SerializeField] private Text callToAction;
        public bool IsRegistered = false;

        public int ID
        {
            get { return _id; }
        }
        public CachedNativeAd CachedNativeAd
        {
            get
            {
                return _cachedNativeAd;
            }
            set
            {
                _cachedNativeAd = value;
                SetData(_cachedNativeAd);
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
            loadedObject.SetActive(false);
            loadingObject.SetActive(true);
            StartCoroutine(HandleShow());
        }

        private IEnumerator HandleShow()
        {
            while (_cachedNativeAd == null)
            {
                yield return null;
            }
            loadedObject.SetActive(true);
            loadingObject.SetActive(false);
            if (_cachedNativeAd.TimeShown == 0)
            {
                FirebaseAnalytics.LogEvent("ad_native_show");
            }
            if (_cachedNativeAd.TimeShown < 2)
            {
                _cachedNativeAd.TimeShown++;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            AdsController.Instance.RegisterNativeAdPanel(this);
            IsRegistered = true;
        }

        private void OnDestroy()
        {
            AdsController.Instance?.UnRegisterNativeAdPanel(this);
        }

        #region HandleNativeAdData
        public void SetData(CachedNativeAd nativeAd)
        {
            _nativeAd = nativeAd.NativeAd;

            if (image != null)
            {
                List<Texture2D> imageTexture2DList = _nativeAd.GetImageTextures();
                if (imageTexture2DList != null)
                {
                    HandleImageTexture2D(imageTexture2DList);
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (icon != null)
            {
                Texture2D iconTexture = _nativeAd.GetIconTexture();
                if (iconTexture != null)
                {
                    HandleIconTexture(iconTexture);
                }
                else
                {
                    icon.gameObject.SetActive(false);
                }
            }

            if (adChoices != null)
            {
                Texture2D adChoicesTexture = _nativeAd.GetAdChoicesLogoTexture();
                if (adChoicesTexture != null)
                {
                    HandleAdChoiceTexture(adChoicesTexture);
                }
                else
                {
                    adChoices.gameObject.SetActive(false);
                }
            }
            if (headline != null)
            {
                string headlineText = _nativeAd.GetHeadlineText();
                if (headlineText != null && headlineText != "")
                {
                    HandleHeadline(headlineText);
                }
                else
                {
                    headline.gameObject.SetActive(false);
                }
            }
            if (body != null)
            {
                string bodyText = _nativeAd.GetBodyText();
                if (bodyText != null && bodyText != "")
                {
                    HandleBody(bodyText);
                }
                else
                {
                    body.gameObject.SetActive(false);
                }
            }

            if (callToAction != null)
            {
                string callToActionText = _nativeAd.GetCallToActionText();
                if (callToActionText != null && callToActionText != "")
                {
                    HandleCallToAction(callToActionText);
                }
            }

            if (rateStar != null)
            {
                double starRating = _nativeAd.GetStarRating();
                if (starRating != 0)
                {
                    HandleStarRating(starRating);
                }
                else
                {
                    rateStar.gameObject.SetActive(false);
                }
            }
        }

        private void HandleStarRating(double starRating)
        {
            Debug.Log(starRating);
        }

        private void HandleCallToAction(string callToActionText)
        {
            callToAction.text = callToActionText;
            if (!_nativeAd.RegisterCallToActionGameObject(callToAction.gameObject))
            {
                Debug.Log("error registering callToAction");
            }
        }

        private void HandleBody(string bodyText)
        {
            body.text = bodyText;

            if (!_nativeAd.RegisterBodyTextGameObject(body.gameObject))
            {
                Debug.Log("error registering body");
            }
        }

        private void HandleHeadline(string headlineText)
        {
            headline.text = headlineText;
            if (!_nativeAd.RegisterHeadlineTextGameObject(headline.gameObject))
            {
                Debug.Log("error registering headline");
            }
        }

        private void HandleAdChoiceTexture(Texture2D adChoicesTexture)
        {
            adChoices.texture = adChoicesTexture;
            if (!_nativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject))
            {
                Debug.Log("error registering adChoices");
            }
        }

        private void HandleIconTexture(Texture2D iconTexture)
        {
            icon.texture = iconTexture;
            if (!_nativeAd.RegisterIconImageGameObject(icon.gameObject))
            {
                Debug.Log("error registering Icon");
            }
        }

        private void HandleImageTexture2D(List<Texture2D> imageTexture2DList)
        {
            Texture2D imageTexture = GetRandom(imageTexture2DList);
            image.texture = imageTexture;
            if (!_nativeAd.RegisterStoreGameObject(image.gameObject))
            {
                Debug.Log("error registering Image");
            }
        }

        private Texture2D GetRandom(List<Texture2D> imageTexture2dList)
        {
            return imageTexture2dList[UnityEngine.Random.Range(0, imageTexture2dList.Count)];
        }
        #endregion
    }
}