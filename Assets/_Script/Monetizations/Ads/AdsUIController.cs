using UnityEngine;
using DG.Tweening;
using Core.UI;
using System;

namespace Monetization.Ads
{
    public class AdsUIController : MonoBehaviour
    {
        [SerializeField] private BasePopup _waitingBox;
        [SerializeField] private BasePopup _rewardUnavailable;
        [SerializeField] private BasePopup _rewardNotRewarded;
        public void ShowWaitingBox()
        {
            _waitingBox.Show();
        }
        public void CloseWaitingBox(Action onClosed)
        {
            _waitingBox.Hide(onClosed);
        }

        public void ShowRewardUnavailableBox()
        {
            _rewardUnavailable.Show();
        }
        public void CloseRewardUnavailableBox()
        {
            _rewardUnavailable.Hide(
                () =>
                {
                    AdsController.Instance.InvokeOnRewarded(false);
                }
                );
        }
        public void ShowNotRewardedBox()
        {
            _rewardNotRewarded.Show();
        }
        public void HideNotRewardedBox()
        {
            _rewardNotRewarded.Hide(() =>
            {
                AdsController.Instance.InvokeOnRewarded(false);
            });
        }

        private void Start()
        {
            AdsController.Instance.RegisterAdsUI(this);
        }
    }
}
