using Core.UI;
using Services.FirebaseService.Analytics;
using Game.Audio;
using Game.Datas;
using Monetization.Ads;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Game.Shared;
using DG.Tweening;
using UnityEngine.UI;
using System.Drawing.Text;

namespace Game.Craft
{
    //TODO bring 
    public class CraftCompleteView : BasePopup
    {
        public SkeletonGraphic Curtain;
        public Button LoseItBtn;
        private void OnEnable()
        {
            LoseItBtn.gameObject.SetActive(false);
            LoseItBtn.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 0);
            //LoseItBtn.GetComponentInChildren<Image>().DOFade(0, 0);
            pointer.GetComponent<Animator>().enabled = true;
            StartCoroutine(ShowLoseItBtn());
        }
        private IEnumerator ShowLoseItBtn()
        {
            yield return new WaitForSecondsRealtime(2);
            LoseItBtn.gameObject.SetActive(true);
            LoseItBtn.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1);
            //LoseItBtn.GetComponentInChildren<Image>().DOFade(1, 1);
        }

        public void OnGetCoin()
        {
            AddGold(10);
        }

        public void OnByAd()
        {

            FirebaseAnalytics.LogEvent("reward_coin_" + CraftSequenceManager.versionName + "_" + CraftSequenceManager.playedTimeCount);
            AdsController.Instance.ShowReward(
                watched =>
                {
                    if (watched)
                    {
                        AddGold(10 * coinMutiplier);
                    }
                }
                );

        }

        private void AddGold(int gold)
        {
            Hide();
            GameDataManager.Instance.UpdateGold(gold);
            GameDataManager.Instance.SaveDatas();
            AudioManager.Instance.PlaySound(SoundID.Coin);
            Invoke("CloseCurtain", 2f);
        }

        public void OnNext()
        {
            FirebaseAnalytics.LogEvent("Replay_" + CraftSequenceManager.versionName + "_" + CraftSequenceManager.playedTimeCount);

            if ((PlayerPrefs.GetInt(Constant.REVIEWED_BY_PLAYER_KEY, 0)) == 0
                && CraftSequenceManager.playedTimeCount >= 2)
            {
                HideImmediately();
                ViewManager.Instance.GetView<RatePanel>().Show(
                    () =>
                    {
                        CloseCurtain();
                        return;
                    }
                    );
            }
            else
            {
                HideImmediately();
                AdsController.Instance.ShowInter(
                    () =>
                    {

                        CloseCurtain();
                    }
                    );
            }
        }
        private void CloseCurtain()
        {
            Curtain.AnimationState.SetAnimation(0, "Open", false);
            Curtain.AnimationState.AddAnimation(0, "Close", false, 1.1f);
            Curtain.AnimationState.AddAnimation(0, "Open_Idle", false, 0);
            Invoke("ResetCraft", 1);
        }
        private void ResetCraft()
        {
            CraftSequenceManager.Instance.ResetCraft();
            AudioManager.Instance.CrossfadeMusic(SoundID.Menu_BGM, .2f);
        }

        public void OnPianoTiles()
        {
            Hide();
            Curtain.AnimationState.SetAnimation(0, "Open", false);
            Curtain.AnimationState.AddAnimation(0, "Close", false, 1.1f);
            Curtain.AnimationState.AddAnimation(0, "Open_Idle", false, 0);
            Invoke("EnterPianoTilesMode", 1);

        }
        private void EnterPianoTilesMode()
        {
            PianoTilesManager.Instance.EnterPianoTilesMode();
        }


        public Transform pointer;
        public int coinMutiplier = 1;
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
                    }
                }
                );
        }
        public TextMeshProUGUI coinText;
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
    }

}