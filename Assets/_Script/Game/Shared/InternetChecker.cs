using Core.UI;
using Monetization.Ads;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InternetChecker : MonoBehaviour
{
    [SerializeField] BasePopup basePopup;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            CheckInternet();
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            CheckInternet();
            yield return new WaitForSecondsRealtime(3);
        }
    }

    public void CheckInternet()
    {
        if (!AdsController.Instance.HasInternet)
        {
            Time.timeScale = 0;
            basePopup.Show();
        }
        else
        {
            Time.timeScale = 1;
            basePopup.Hide();
        }
    }

    public void OpenInternetSetting()
    {
        try
        {
#if UNITY_ANDROID
            Debug.Log("Open setting");
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject =
                unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var intentObject = new AndroidJavaObject(
                "android.content.Intent", "android.settings.WIFI_SETTINGS"))
            {
                currentActivityObject.Call("startActivity", intentObject);
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }


}
