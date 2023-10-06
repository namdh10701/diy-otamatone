using Core.UI;
using Game;
using Game.Audio;
using Game.Settings;
using Monetization.Ads.UI;
using TMPro;
using UnityEngine;
using static Core.UI.ToggleButton;

public class SettingPanel : InterPopup
{
    [SerializeField] private ToggleButton _musicButton;
    [SerializeField] private ToggleButton _soundButton;
    [SerializeField] private ToggleButton _vibrateButton;
    //[SerializeField] private NativeAdPanel _nativeAdPanel;
    [SerializeField] private TextMeshProUGUI _versionName;
    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        //StartCoroutine(WaitAndShowNativeAds());
    }
    private void OnDisable()
    {
        //AdsController.Instance?.HideNativeAd(_nativeAdPanel);
    }
    public void Init()
    {
        _musicButton.Init(SettingManager.Instance.GameSettings.IsMusicOn ? ToggleState.ON : ToggleState.OFF);
        _soundButton.Init(SettingManager.Instance.GameSettings.IsSoundOn ? ToggleState.ON : ToggleState.OFF);
        _vibrateButton.Init(SettingManager.Instance.GameSettings.IsVibrationOn ? ToggleState.ON : ToggleState.OFF);

        _versionName.text = "Version " + Application.version.ToString();
    }

    public void SetMusic(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsMusicOn = isOn;
        SettingManager.Instance.SaveSettings();
        AudioManager.Instance.IsMusicOn = isOn;
    }
    public void SetSound(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsSoundOn = isOn;
        SettingManager.Instance.SaveSettings();
        AudioManager.Instance.IsSoundOn = isOn;
    }
    public void SetVibrate(bool isOn)
    {
        SettingManager.Instance.GameSettings.IsVibrationOn = isOn;
        Vibration.SetState(isOn);
        SettingManager.Instance.SaveSettings();
        if (isOn)
            Vibration.Vibrate(100);
    }


    /*private IEnumerator WaitAndShowNativeAds()
    {
        while (!_nativeAdPanel.IsRegistered)
        {
            yield return null;
        }
        if (AdsHandler.AdRemoved())
        {
            Debug.Log("Hide native");
            AdsController.Instance.HideNativeAd(_nativeAdPanel);
            yield break;
        }
        else
        {
            Debug.Log("show native ads from setting");
            AdsController.Instance.ShowNativeAd(_nativeAdPanel);
        }
    }*/
}