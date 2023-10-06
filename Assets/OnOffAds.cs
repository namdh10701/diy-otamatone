using Monetization.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnOffAds : MonoBehaviour
{
    void Start()
    {
        //_button.onClick.AddListener(TurnOffAds);
    }

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    private void TurnOffAds()
    {
        _text.text = "Turn on ads";
        //AdsController.Instance.OnRemoveAds(true);
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(TurnOnAds);
    }

    private void TurnOnAds()
    {
        _text.text = "Turn off ads";
        //AdsController.Instance.OnRemoveAds(false);
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(TurnOffAds);
    }
}
