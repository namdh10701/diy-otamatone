using Core.UI;
using Monetization.Ads;
using System.Collections;
using UnityEngine;

public class InternetChecker : MonoBehaviour
{
    [SerializeField] BasePopup basePopup;
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
}
