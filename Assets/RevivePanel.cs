using Core.UI;
using Monetization.Ads;
using UnityEngine;

public class RevivePanel : BasePopup
{
    public void OnCountdownFinished()
    {
        HideImmediately();
        PVPManager.Instance.OnLevelLose();
    }

    public void OnRevive()
    {
        Hide();
        PVPManager.Instance.Revive();
        /* AdsController.Instance.ShowReward(
             watched =>
             {
                 if (watched)
                 {
                     PVPManager.Instance.Revive();
                 }
             }
             );*/
    }
}
