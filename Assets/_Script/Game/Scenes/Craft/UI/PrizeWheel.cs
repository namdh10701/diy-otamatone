using Core.UI;
using DG.Tweening;
using Game.Audio;
using Game.Craft;
using Game.Datas;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeWheel : MonoBehaviour
{
    public Button LoseItBtn;
    public Transform pointer;
    public int coinMutiplier = 1;
    public TextMeshProUGUI coinText;
    public BasePopup rewardRecieved;
    private void OnEnable()
    {
        if (LoseItBtn != null)
        {
            LoseItBtn.gameObject.SetActive(false);
            LoseItBtn.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 0);
            StartCoroutine(ShowLoseItBtn());
        }
        pointer.GetComponent<Animator>().enabled = true;
    }
    private IEnumerator ShowLoseItBtn()
    {
        yield return new WaitForSecondsRealtime(2);
        if (LoseItBtn != null)
        {
            LoseItBtn.gameObject.SetActive(true);
            LoseItBtn.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1);
        }
    }
    private void Update()
    {
        //Quaternion rotation = pointer.localEulerAngles;
        float z = pointer.localEulerAngles.z;
        if (z > 180)
        {
            z -= 360;
        }
        if ((z <= 120 && z > 78) || (z >= -120 && z < -78))
        {
            coinMutiplier = 2;
            coinText.text = "+20";
        }
        else if (z <= 78 && z > 37 || (z >= -78 && z < -37))
        {
            coinMutiplier = 3;
            coinText.text = "+30";
        }
        else if (z <= 37 && z > 11 || (z >= -37 && z < -11))
        {
            coinMutiplier = 5;
            coinText.text = "+50";
        }
        else if (z <= 11 && z > -11)
        {
            coinMutiplier = 10;
            coinText.text = "+100";
        }
    }
    public void StopSpin()
    {
        pointer.GetComponent<Animator>().enabled = false;
        FirebaseAnalytics.LogEvent("reward_coin_" + CraftSequenceManager.versionName + "_" + CraftSequenceManager.playedTimeCount);
        AdsController.Instance.ShowReward(
            watched =>
            {
                if (watched)
                {
                    AddGold(10 * coinMutiplier);
                    rewardRecieved.Show();
                    gameObject.SetActive(false);
                }
            }
            );
    }

    public void AddGold(int gold)
    {
        GameDataManager.Instance.UpdateGold(gold);
        GameDataManager.Instance.SaveDatas();
        AudioManager.Instance.PlaySound(SoundID.Coin);
    }
}