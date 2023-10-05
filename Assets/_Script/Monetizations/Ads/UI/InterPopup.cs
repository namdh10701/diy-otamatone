using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Core.UI;

namespace Monetization.Ads.UI
{
    public class InterPopup : BasePopup
    {
        [SerializeField] protected bool _isShowAd;

        protected override void Awake()
        {
            base.Awake();
            _panel?.GetComponent<Button>().onClick.RemoveAllListeners();
            _panel?.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Hide(_isShowAd);
               }
               );
        }
        public void Show(bool showAd = true)
        {
            if (showAd)
                AdsController.Instance.ShowInter(
                     () =>
                     {
                         base.Show();
                     });
            else
                base.Show();

        }

        public void Hide(bool showAd = true)
        {
            if (showAd)
                AdsController.Instance.ShowInter(
                   () =>
                   {
                       base.Hide();
                   });
            else
                base.Hide();
        }
    }
}