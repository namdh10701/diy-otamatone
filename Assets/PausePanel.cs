using Core.UI;
using Monetization.Ads;
using System;
using UnityEngine;

public class PausePanel : BasePopup
{
    Action onclosed;
    public void ShowWithCallback(Action onCloseCallback)
    {
        onclosed = onCloseCallback;
        AdsController.Instance.ShowInter(
            () =>
            {
                base.Show();
            }
            );
    }

    public void ShowInterAndHide()
    {
        AdsController.Instance.ShowInter(
            () =>
            {
                base.Hide(onclosed);
            }
            );
    }
}
