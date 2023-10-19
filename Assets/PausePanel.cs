using Core.UI;
using Monetization.Ads;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnResume()
    {
        HideImmediately();
        PlayBotManager.Instance.OnContinue();
    }
    public void OnReplay()
    {
        AdsController.Instance.ShowInter(
           () =>
           {
               base.HideImmediately();
               PlayBotManager.Instance.Replay();
           }
           );
    }
    public void OnHome()
    {
        AdsController.Instance.ShowInter(
            () =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("HomeScene");
            }
            );
    }
}
